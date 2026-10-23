using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Exceptions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Buffers;
using System.IO;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.JsonRpc.Clients;

/// <summary>ElectrumX JSON-RPC client over a WebSocket endpoint.</summary>
public class ElectrumXWebSocketRpcClient : IElectrumXRpcClient
{
    private const int ReceiveBufferSize = 8192;
    private readonly IElectrumXRpcClientOptions _clientOptions;
    private readonly ElectrumXWebSocketOptionsConfigurator? _configureWebSocketOptions;
    private readonly Uri _endpoint;

    private readonly ElectrumXRpcSession _session;
    private int _connected;
    private int _disposed;
    private Task? _receiveTask;
    private ClientWebSocket? _webSocket;

    /// <summary>Creates a WebSocket client using ws://{Host}:{Port} by default.</summary>
    /// <param name="clientOptions">The ElectrumX RPC options.</param>
    /// <param name="configureWebSocketOptions">Optional callback to configure WebSocket options.</param>
    public ElectrumXWebSocketRpcClient(
        IElectrumXRpcClientOptions clientOptions,
        ElectrumXWebSocketOptionsConfigurator? configureWebSocketOptions = null)
        : this(clientOptions, CreateEndpoint(clientOptions, "ws"), configureWebSocketOptions)
    {
    }

    /// <summary>Creates a WebSocket client using the specified endpoint.</summary>
    /// <param name="clientOptions">The ElectrumX RPC options.</param>
    /// <param name="endpoint">The WebSocket endpoint.</param>
    /// <param name="configureWebSocketOptions">Optional callback to configure WebSocket options.</param>
    protected ElectrumXWebSocketRpcClient(
        IElectrumXRpcClientOptions clientOptions,
        Uri endpoint,
        ElectrumXWebSocketOptionsConfigurator? configureWebSocketOptions)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(clientOptions, nameof(clientOptions));
        Abstractions.ArgumentGuard.ThrowIfNull(endpoint, nameof(endpoint));

        _clientOptions = clientOptions;
        _endpoint = endpoint;
        _configureWebSocketOptions = configureWebSocketOptions;

        _session = new ElectrumXRpcSession(clientOptions, new RequestSender(this));
        _session.BlockHeaderReceived += (_, args) =>
            ElectrumXNotificationPublisher.Publish(BlockHeaderReceived, this, args);
        _session.BlockHeightReceived += (_, args) =>
            ElectrumXNotificationPublisher.Publish(BlockHeightReceived, this, args);
        _session.ScriptHashStatusChanged += (_, args) =>
            ElectrumXNotificationPublisher.Publish(ScriptHashStatusChanged, this, args);
        _session.MasterNodeStatusChanged += (_, args) =>
            ElectrumXNotificationPublisher.Publish(MasterNodeStatusChanged, this, args);
        _session.AddressStatusChanged += (_, args) =>
            ElectrumXNotificationPublisher.Publish(AddressStatusChanged, this, args);
        _session.TransactionStatusChanged += (_, args) =>
            ElectrumXNotificationPublisher.Publish(TransactionStatusChanged, this, args);
        _session.DoubleSpendProofStatusChanged += (_, args) =>
            ElectrumXNotificationPublisher.Publish(DoubleSpendProofStatusChanged, this, args);
    }

    /// <summary>The WebSocket endpoint used by this client.</summary>
    protected Uri Endpoint => _endpoint;

    /// <inheritdoc />
    public IElectrumXRpcClientOptions ClientOptions => _clientOptions;

    /// <inheritdoc />
    public ElectrumXServerVersion ServerVersion { get; private set; } = new("", new ElectrumXProtocolVersion(0, 0));

    /// <inheritdoc />
    public bool IsConnected =>
        _webSocket?.State == WebSocketState.Open &&
        _session.IsAvailable &&
        ServerVersion.ProtocolVersion >= new ElectrumXProtocolVersion(1, 0);

    /// <inheritdoc />
    public event EventHandler<ElectrumXBlockHeaderEventArgs>? BlockHeaderReceived;

    /// <inheritdoc />
    public event EventHandler<ElectrumXBlockHeightEventArgs>? BlockHeightReceived;

    /// <inheritdoc />
    public event EventHandler<ElectrumXScriptHashStatusEventArgs>? ScriptHashStatusChanged;

    /// <inheritdoc />
    public event EventHandler<ElectrumXMasterNodeStatusEventArgs>? MasterNodeStatusChanged;

    public event EventHandler<ElectrumXAddressNotificationEventArgs>? AddressStatusChanged;

    public event EventHandler<ElectrumXTransactionNotificationEventArgs>? TransactionStatusChanged;

    public event EventHandler<ElectrumXDoubleSpendProofNotificationEventArgs>? DoubleSpendProofStatusChanged;

    /// <inheritdoc />
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        if (Interlocked.Exchange(ref _connected, 1) != 0)
        {
            throw new InvalidOperationException("The WebSocket client has already started connecting.");
        }

        using var connectTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        connectTimeout.CancelAfter(ClientOptions.ConnectTimeout);
        var webSocket = new ClientWebSocket();
        _webSocket = webSocket;

        try
        {
            ConfigureWebSocket(webSocket);
            await webSocket.ConnectAsync(Endpoint, connectTimeout.Token).ConfigureAwait(false);

            _receiveTask = ReceiveMessagesAsync(webSocket, _session.ShutdownToken);
            ServerVersion = await ElectrumXRpcNegotiator.NegotiateAsync(this, connectTimeout.Token)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException exception)
            when (connectTimeout.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
        {
            await DisposeAsync().ConfigureAwait(false);
            throw new ElectrumXTimeoutException("Connecting to the ElectrumX server timed out.", exception);
        }
        catch (Exception exception) when (IsTransportFailure(exception))
        {
            await DisposeAsync().ConfigureAwait(false);
            throw new ElectrumXConnectionException("Could not connect to the ElectrumX server.", exception);
        }
        catch
        {
            await DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    /// <inheritdoc />
    public Task<JsonRpcResponse<TResponse>> CallAsync<TResponse, TQuery>(
        JsonRpcRequest<TQuery> request,
        CancellationToken cancellationToken = default)
        where TResponse : ElectrumXResult
        where TQuery : ElectrumXQuery
    {
        return _session.CallAsync<TResponse, TQuery>(request, cancellationToken);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        try
        {
            await _session.DisposeAsync().ConfigureAwait(false);
            _webSocket?.Abort();
            if (_receiveTask != null)
            {
                await _receiveTask.ConfigureAwait(false);
            }
        }
        finally
        {
            _webSocket?.Dispose();
        }
    }


    private async ValueTask SendRequestAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
    {
        var socket = _webSocket ?? throw new InvalidOperationException("The WebSocket connection is not open.");
        await socket.SendAsync(payload, WebSocketMessageType.Text, true, cancellationToken).ConfigureAwait(false);
    }

    private async Task ReceiveMessagesAsync(ClientWebSocket socket, CancellationToken cancellationToken)
    {
        byte[] receiveBuffer = new byte[ReceiveBufferSize];
        var message = new ArrayBufferWriter<byte>(ReceiveBufferSize);
        try
        {
            while (true)
            {
                message.Clear();
                ValueWebSocketReceiveResult receiveResult;
                do
                {
                    receiveResult = await socket.ReceiveAsync(receiveBuffer.AsMemory(), cancellationToken)
                        .ConfigureAwait(false);
                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        if (socket.State == WebSocketState.CloseReceived)
                        {
                            await socket.CloseOutputAsync(
                                socket.CloseStatus ?? WebSocketCloseStatus.NormalClosure,
                                socket.CloseStatusDescription,
                                CancellationToken.None).ConfigureAwait(false);
                        }

                        _session.Fail(new EndOfStreamException("The ElectrumX WebSocket server closed the connection."));
                        return;
                    }

                    if (receiveResult.MessageType != WebSocketMessageType.Text)
                    {
                        throw new ElectrumXProtocolException("The ElectrumX WebSocket server sent a non-text message.");
                    }

                    receiveBuffer.AsSpan(0, receiveResult.Count).CopyTo(message.GetSpan(receiveResult.Count));
                    message.Advance(receiveResult.Count);
                } while (!receiveResult.EndOfMessage);

                _session.ProcessMessage(message.WrittenMemory);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception exception)
        {
            _session.Fail(exception);
        }
    }

    /// <summary>Configures transport-specific options before opening the WebSocket connection.</summary>
    /// <param name="webSocket">The WebSocket being prepared for connection.</param>
    protected virtual void ConfigureWebSocket(ClientWebSocket webSocket)
    {
        _configureWebSocketOptions?.Invoke(webSocket.Options);
    }

    /// <summary>Creates a WebSocket endpoint using the configured host and port.</summary>
    /// <param name="clientOptions">The ElectrumX RPC options.</param>
    /// <param name="scheme">The WebSocket URI scheme.</param>
    /// <returns>The endpoint URI.</returns>
    protected static Uri CreateEndpoint(IElectrumXRpcClientOptions clientOptions, string scheme)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(clientOptions, nameof(clientOptions));
        string host = ArgumentGuard.NormalizeAndValidateHost(clientOptions.Host, nameof(clientOptions.Host));
        return new UriBuilder(scheme, host, clientOptions.Port).Uri;
    }

    private void ThrowIfDisposed()
    {
        if (Interlocked.CompareExchange(ref _disposed, 0, 0) != 0)
        {
            throw new ObjectDisposedException(GetType().FullName);
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

    private sealed class RequestSender : IElectrumXRpcRequestSender
    {
        private readonly ElectrumXWebSocketRpcClient _client;

        internal RequestSender(ElectrumXWebSocketRpcClient client)
        {
            _client = client;
        }

        public ValueTask SendAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
        {
            return _client.SendRequestAsync(payload, cancellationToken);
        }
    }
}
