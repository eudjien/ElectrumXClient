using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Exceptions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Buffers;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.JsonRpc.Clients;

/// <summary>Base JSON-RPC transport for ElectrumX clients that communicate over TCP streams.</summary>
public abstract class ElectrumXTcpRpcClientBase : IElectrumXRpcClient
{
    private static readonly byte[] NewLine = [(byte)'\n'];

    private readonly IElectrumXRpcClientOptions _clientOptions;
    private readonly ArrayBufferWriter<byte> _lineBuffer = new(4096);
    private readonly byte[] _responseBuffer = new byte[4096];
    private readonly ElectrumXRpcSession _session;
    private int _disposed;
    private Task? _readerTask;
    private int _responseBufferLength;
    private int _responseBufferPosition;
    private Stream? _stream;

    /// <summary>Creates a TCP-stream JSON-RPC client using the configured host and port.</summary>
    protected ElectrumXTcpRpcClientBase(IElectrumXRpcClientOptions clientOptions)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(clientOptions, nameof(clientOptions));
        _clientOptions = clientOptions;
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

    private Stream ConnectedStream =>
        Interlocked.CompareExchange(ref _stream, null, null) ??
        throw new InvalidOperationException("The ElectrumX connection is not open.");

    public IElectrumXRpcClientOptions ClientOptions => _clientOptions;

    public event EventHandler<ElectrumXBlockHeaderEventArgs>? BlockHeaderReceived;

    public event EventHandler<ElectrumXBlockHeightEventArgs>? BlockHeightReceived;

    public event EventHandler<ElectrumXScriptHashStatusEventArgs>? ScriptHashStatusChanged;

    public event EventHandler<ElectrumXMasterNodeStatusEventArgs>? MasterNodeStatusChanged;

    public event EventHandler<ElectrumXAddressNotificationEventArgs>? AddressStatusChanged;

    public event EventHandler<ElectrumXTransactionNotificationEventArgs>? TransactionStatusChanged;

    public event EventHandler<ElectrumXDoubleSpendProofNotificationEventArgs>? DoubleSpendProofStatusChanged;

    /// <inheritdoc />
    public ElectrumXServerVersion ServerVersion { get; private set; } = new("", new ElectrumXProtocolVersion(0, 0));

    /// <inheritdoc />
    public bool IsConnected =>
        Interlocked.CompareExchange(ref _stream, null, null) is not null &&
        _session.IsAvailable &&
        ServerVersion.ProtocolVersion >= new ElectrumXProtocolVersion(1, 0);

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        try
        {
            await _session.DisposeAsync().ConfigureAwait(false);

            if (Interlocked.Exchange(ref _stream, null) is {} stream)
            {
                await stream.DisposeAsync().ConfigureAwait(false);
            }

            if (_readerTask != null)
            {
                await _readerTask.ConfigureAwait(false);
            }
        }
        finally
        {
            if (Interlocked.Exchange(ref _stream, null) is {} stream)
            {
                await stream.DisposeAsync().ConfigureAwait(false);
            }

            GC.SuppressFinalize(this);
        }
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        if (Interlocked.CompareExchange(ref _stream, null, null) is not null)
        {
            throw new InvalidOperationException("The JSON-RPC client is already connected.");
        }

        using var connectTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        connectTimeout.CancelAfter(ClientOptions.ConnectTimeout);
        string host = ArgumentGuard.NormalizeAndValidateHost(ClientOptions.Host, nameof(ClientOptions.Host));

        try
        {
            await ConnectTransportAsync(host, connectTimeout.Token).ConfigureAwait(false);
        }
        catch (Exception exception) when (connectTimeout.IsCancellationRequested)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException("Connecting to the ElectrumX server was canceled.", exception, cancellationToken);
            }

            throw new ElectrumXTimeoutException("Connecting to the ElectrumX server timed out.", exception);
        }
        catch (Exception exception) when (IsTransportFailure(exception))
        {
            throw new ElectrumXConnectionException("Could not connect to the ElectrumX server.", exception);
        }

        try
        {
            _readerTask = ReadResponsesAsync(_session.ShutdownToken);
            ServerVersion = await ElectrumXRpcNegotiator.NegotiateAsync(this, connectTimeout.Token)
                .ConfigureAwait(false);
        }
        catch (OperationCanceledException exception) when (!cancellationToken.IsCancellationRequested)
        {
            await DisposeAsync().ConfigureAwait(false);
            throw new ElectrumXTimeoutException("Connecting to the ElectrumX server timed out.", exception);
        }
        catch (Exception exception) when (IsTransportFailure(exception))
        {
            await DisposeAsync().ConfigureAwait(false);
            throw new ElectrumXConnectionException("The ElectrumX connection failed during negotiation.", exception);
        }
        catch
        {
            await DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    /// <summary>Sends a JSON-RPC request and deserializes its result.</summary>
    public Task<JsonRpcResponse<TResponse>> CallAsync<TResponse, TQuery>(
        JsonRpcRequest<TQuery> request,
        CancellationToken cancellationToken = default)
        where TResponse : ElectrumXResult
        where TQuery : ElectrumXQuery
    {
        return _session.CallAsync<TResponse, TQuery>(request, cancellationToken);
    }

    private async Task ConnectTransportAsync(string host, CancellationToken cancellationToken)
    {
        var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        Stream? stream = null;
        bool connected = false;

        try
        {
            await using (cancellationToken.Register(socket.Dispose))
            {
                await socket.ConnectAsync(new DnsEndPoint(host, ClientOptions.Port)).ConfigureAwait(false);
            }

            var networkStream = new NetworkStream(socket, true);
            stream = await CreateConnectedStreamAsync(networkStream, cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            if (Interlocked.CompareExchange(ref _stream, stream, null) is not null)
            {
                throw new InvalidOperationException("The JSON-RPC client is already connected.");
            }

            connected = true;
        }
        finally
        {
            if (!connected)
            {
                if (stream is not null)
                {
                    await stream.DisposeAsync().ConfigureAwait(false);
                }

                socket.Dispose();
            }
        }
    }

    private async ValueTask SendRequestAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
    {
        var stream = ConnectedStream;
        await stream.WriteAsync(payload, cancellationToken).ConfigureAwait(false);
        await stream.WriteAsync(NewLine, cancellationToken).ConfigureAwait(false);
        await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task ReadResponsesAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (true)
            {
                var line = await ReadLineAsync(cancellationToken).ConfigureAwait(false);
                if (line is null)
                {
                    throw new EndOfStreamException("The ElectrumX server closed the connection.");
                }

                if (line.Value.Length > 0)
                {
                    _session.ProcessMessage(line.Value);
                }
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

    private async ValueTask<ReadOnlyMemory<byte>?> ReadLineAsync(CancellationToken cancellationToken)
    {
        _lineBuffer.Clear();

        while (true)
        {
            if (_responseBufferPosition == _responseBufferLength)
            {
                _responseBufferLength = await ConnectedStream.ReadAsync(_responseBuffer.AsMemory(), cancellationToken)
                    .ConfigureAwait(false);
                _responseBufferPosition = 0;

                if (_responseBufferLength == 0)
                {
                    return _lineBuffer.WrittenCount == 0 ? null : _lineBuffer.WrittenMemory;
                }
            }

            int newlineIndex = Array.IndexOf(
                _responseBuffer,
                (byte)'\n',
                _responseBufferPosition,
                _responseBufferLength - _responseBufferPosition);
            int segmentEnd = newlineIndex >= 0 ? newlineIndex : _responseBufferLength;
            int segmentLength = segmentEnd - _responseBufferPosition;

            _responseBuffer.AsSpan(_responseBufferPosition, segmentLength)
                .CopyTo(_lineBuffer.GetSpan(segmentLength));
            _lineBuffer.Advance(segmentLength);
            _responseBufferPosition = newlineIndex >= 0 ? newlineIndex + 1 : segmentEnd;

            if (newlineIndex >= 0)
            {
                int lineLength = _lineBuffer.WrittenCount;
                if (lineLength > 0 && _lineBuffer.WrittenSpan[lineLength - 1] == '\r')
                {
                    lineLength--;
                }

                return _lineBuffer.WrittenMemory[..lineLength];
            }
        }
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
                   IOException or
                   AuthenticationException;
    }

    /// <summary>Creates the stream used for JSON-RPC after the TCP socket is connected.</summary>
    protected virtual ValueTask<Stream> CreateConnectedStreamAsync(
        NetworkStream networkStream,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return new ValueTask<Stream>(networkStream);
    }

    private sealed class RequestSender : IElectrumXRpcRequestSender
    {
        private readonly ElectrumXTcpRpcClientBase _client;

        internal RequestSender(ElectrumXTcpRpcClientBase client)
        {
            _client = client;
        }

        public ValueTask SendAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken)
        {
            return _client.SendRequestAsync(payload, cancellationToken);
        }
    }
}
