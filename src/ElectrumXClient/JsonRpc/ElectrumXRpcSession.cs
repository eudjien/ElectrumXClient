using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Exceptions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.JsonRpc;

internal sealed class ElectrumXRpcSession : IAsyncDisposable
{
    private readonly IElectrumXRpcClientOptions _clientOptions;
    private readonly IElectrumXJsonConverter _converter;
    private readonly ElectrumXNotificationDispatcher _notificationDispatcher;
    private readonly IElectrumXRpcRequestSender _requestSender;
    private readonly ElectrumXRpcRequestTracker _requestTracker;
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    private readonly CancellationTokenSource _shutdown = new();
    private readonly CancellationToken _shutdownToken;
    private Exception? _connectionError;
    private int _disposed;

    internal ElectrumXRpcSession(
        IElectrumXRpcClientOptions clientOptions,
        IElectrumXRpcRequestSender requestSender)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(clientOptions, nameof(clientOptions));
        Abstractions.ArgumentGuard.ThrowIfNull(requestSender, nameof(requestSender));

        _clientOptions = clientOptions;
        _converter = clientOptions.Converter;
        _requestSender = requestSender;
        _notificationDispatcher = new ElectrumXNotificationDispatcher(_converter);
        _requestTracker = new ElectrumXRpcRequestTracker();
        _shutdownToken = _shutdown.Token;
    }

    internal CancellationToken ShutdownToken => _shutdownToken;

    internal bool IsAvailable =>
        Interlocked.CompareExchange(ref _disposed, 0, 0) == 0 &&
        Interlocked.CompareExchange(ref _connectionError, null, null) is null;

    public ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return default;
        }

        _requestTracker.CompleteAllWithException(new ObjectDisposedException(nameof(ElectrumXRpcSession)));
        _shutdown.Cancel();
        _shutdown.Dispose();
        return default;
    }

    internal event EventHandler<ElectrumXBlockHeaderEventArgs>? BlockHeaderReceived
    {
        add => _notificationDispatcher.BlockHeaderReceived += value;
        remove => _notificationDispatcher.BlockHeaderReceived -= value;
    }

    internal event EventHandler<ElectrumXBlockHeightEventArgs>? BlockHeightReceived
    {
        add => _notificationDispatcher.BlockHeightReceived += value;
        remove => _notificationDispatcher.BlockHeightReceived -= value;
    }

    internal event EventHandler<ElectrumXScriptHashStatusEventArgs>? ScriptHashStatusChanged
    {
        add => _notificationDispatcher.ScriptHashStatusChanged += value;
        remove => _notificationDispatcher.ScriptHashStatusChanged -= value;
    }

    internal event EventHandler<ElectrumXMasterNodeStatusEventArgs>? MasterNodeStatusChanged
    {
        add => _notificationDispatcher.MasterNodeStatusChanged += value;
        remove => _notificationDispatcher.MasterNodeStatusChanged -= value;
    }

    internal event EventHandler<ElectrumXAddressNotificationEventArgs>? AddressStatusChanged
    {
        add => _notificationDispatcher.AddressNotificationReceived += value;
        remove => _notificationDispatcher.AddressNotificationReceived -= value;
    }

    internal event EventHandler<ElectrumXTransactionNotificationEventArgs>? TransactionStatusChanged
    {
        add => _notificationDispatcher.TransactionNotificationReceived += value;
        remove => _notificationDispatcher.TransactionNotificationReceived -= value;
    }

    internal event EventHandler<ElectrumXDoubleSpendProofNotificationEventArgs>? DoubleSpendProofStatusChanged
    {
        add => _notificationDispatcher.DoubleSpendProofNotificationReceived += value;
        remove => _notificationDispatcher.DoubleSpendProofNotificationReceived -= value;
    }

    internal async Task<JsonRpcResponse<TResponse>> CallAsync<TResponse, TQuery>(
        JsonRpcRequest<TQuery> request,
        CancellationToken cancellationToken)
        where TResponse : ElectrumXResult
        where TQuery : ElectrumXQuery
    {
        Abstractions.ArgumentGuard.ThrowIfNull(request, nameof(request));

        ThrowIfUnavailable();

        var registration = RegisterRequest(request.Id);

        try
        {
            using var requestTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _shutdownToken);
            requestTimeout.CancelAfter(_clientOptions.RequestTimeout);

            await SendRequestAsync(
                registration.Id,
                request,
                requestTimeout.Token).ConfigureAwait(false);

            var response = await WaitForResponseAsync(registration, requestTimeout.Token).ConfigureAwait(false);

            return DeserializeResponse<TResponse>(response);
        }
        catch (OperationCanceledException exception)
            when (!cancellationToken.IsCancellationRequested && !_shutdownToken.IsCancellationRequested)
        {
            throw new ElectrumXTimeoutException(
                "The ElectrumX request '{0}' timed out.",
                exception,
                request.Method);
        }
        catch (Exception exception) when (IsTransportFailure(exception))
        {
            throw new ElectrumXConnectionException("The ElectrumX connection failed while executing '{0}'.", exception, request.Method);
        }
        finally
        {
            _requestTracker.Remove(registration.Id);
        }
    }

    private async Task SendRequestAsync<TQuery>(JsonRpcId id, JsonRpcRequest<TQuery> request, CancellationToken cancellationToken)
        where TQuery : ElectrumXQuery
    {
        var requestToSend = request.WithId(id);

        byte[] payload;
        try
        {
            payload = _converter.SerializeRequest(requestToSend);
        }
        catch (ElectrumXProtocolException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new ElectrumXProtocolException("The JSON-RPC request could not be serialized.", exception);
        }

        await _sendLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            ThrowIfUnavailable();
            await _requestSender.SendAsync(payload, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _sendLock.Release();
        }
    }

    private static async Task<Response> WaitForResponseAsync(ElectrumXRpcRequestRegistration registration, CancellationToken cancellationToken)
    {
        await using (cancellationToken.Register(_ => { registration.CompletionSource.TrySetCanceled(); }, null))
        {
            return await registration.CompletionSource.Task.ConfigureAwait(false);
        }
    }

    private JsonRpcResponse<TResponse> DeserializeResponse<TResponse>(Response response) where TResponse : ElectrumXResult
    {
        JsonRpcResponse<TResponse> rpcResponse;
        try
        {
            rpcResponse = _converter.DeserializeResponse<TResponse>(response.Payload);
        }
        catch (ElectrumXProtocolException)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC response.", exception);
        }

        if (rpcResponse.Error is not null)
        {
            var errorResponse = JsonRpcResponse<ElectrumXResult>.CreateError(rpcResponse.Id, rpcResponse.JsonRpcVersion, rpcResponse.Error);
            throw new ElectrumXServerException(errorResponse);
        }

        return rpcResponse;
    }

    internal void ProcessMessage(ReadOnlyMemory<byte> message)
    {
        JsonRpcHeader header;
        try
        {
            header = _converter.DeserializeHeader(message);
        }
        catch (ElectrumXProtocolException)
        {
            return;
        }

        try
        {
            var messageType = ClassifyMessage(header);
            if (messageType == JsonRpcHeader.JsonRpcMessageType.Notification)
            {
                _notificationDispatcher.Dispatch(header.Method, message);
                return;
            }

            var id = header.Id;
            if (messageType != JsonRpcHeader.JsonRpcMessageType.Response ||
                id is null ||
                id.Kind == JsonRpcIdKind.Null)
            {
                return;
            }

            _requestTracker.Complete(id, message);
        }
        catch (ElectrumXProtocolException)
        {
        }
    }

    internal void Fail(Exception exception)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(exception, nameof(exception));

        var failure = IsTransportFailure(exception)
            ? new ElectrumXConnectionException("The ElectrumX connection was lost.", exception)
            : exception;

        var firstFailure = Interlocked.CompareExchange(ref _connectionError, failure, null) ?? failure;
        _requestTracker.CompleteAllWithException(firstFailure);
    }

    private ElectrumXRpcRequestRegistration RegisterRequest(JsonRpcId id)
    {
        ThrowIfUnavailable();

        var registration = _requestTracker.Register(id);

        try
        {
            ThrowIfUnavailable();
            return registration;
        }
        catch
        {
            _requestTracker.Remove(registration.Id);
            throw;
        }
    }

    private static JsonRpcHeader.JsonRpcMessageType ClassifyMessage(JsonRpcHeader header)
    {
        if (header.JsonRpcVersion is null)
        {
            throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC message.");
        }

        if (header.JsonRpcVersion != ElectrumXJsonPropertyValues.JsonRpcVersion)
        {
            throw new ElectrumXProtocolException(
                "Unsupported JSON-RPC version '{0}'. Expected '{1}'.",
                header.JsonRpcVersion,
                ElectrumXJsonPropertyValues.JsonRpcVersion);
        }

        if (header is { HasId: true, Id: null } ||
            header is { HasMethod: true, Method: null } ||
            header.ParametersKind == JsonRpcHeader.JsonRpcParametersKind.Other)
        {
            throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC message.");
        }

        if (header.HasMethod)
        {
            if (header.HasError || header.HasResult)
            {
                throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC message.");
            }

            return header.HasId && header.Id?.Kind != JsonRpcIdKind.Null
                ? JsonRpcHeader.JsonRpcMessageType.Request
                : header.HasId
                    ? throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC message.")
                    : JsonRpcHeader.JsonRpcMessageType.Notification;
        }

        if (!header.HasId || header.HasResult == header.HasError ||
            header is { HasError: true, IsErrorObject: false } ||
            header.Id?.Kind == JsonRpcIdKind.Null && !header.HasError)
        {
            throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC message.");
        }

        return JsonRpcHeader.JsonRpcMessageType.Response;
    }

    private void ThrowIfUnavailable()
    {
        if (Interlocked.CompareExchange(ref _disposed, 0, 0) != 0)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }

        if (Interlocked.CompareExchange(ref _connectionError, null, null) is {} connectionError)
        {
            throw connectionError;
        }
    }

    private static bool IsTransportFailure(Exception exception)
    {
        return exception is not ElectrumXException &&
               exception is SocketException or
                   WebSocketException or
                   IOException or
                   AuthenticationException;
    }
}
