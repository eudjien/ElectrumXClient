using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Clients;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;
using ElectrumXRpcClientOptions=NorthGomel.ElectrumXClient.JsonRpc.ElectrumXRpcClientOptions;

namespace NorthGomel.ElectrumXClient.Clients;

/// <inheritdoc />
public sealed class ElectrumXClient : IElectrumXClient
{
    private readonly IElectrumXBlockchainClient _blockchain;
    private readonly IElectrumXMasterNodeClient _masterNode;
    private readonly IElectrumXMempoolClient _mempool;

    private readonly bool _ownsRpcClient;
    private readonly IElectrumXProTxClient _proTx;
    private readonly IElectrumXRpcClient _rpcClient;
    private readonly IElectrumXServerClient _server;
    private int _disposed;

    internal ElectrumXClient(IElectrumXRpcClient rpcClient, bool ownsRpcClient = true)
    {
        _rpcClient = rpcClient ?? throw new ArgumentNullException(nameof(rpcClient));
        _ownsRpcClient = ownsRpcClient;
        _blockchain = new ElectrumXBlockchainClient(_rpcClient, false);
        _server = new ElectrumXServerClient(_rpcClient, false);
        _masterNode = new ElectrumXMasterNodeClient(_rpcClient, false);
        _proTx = new ElectrumXProTxClient(_rpcClient, false);
        _mempool = new ElectrumXMempoolClient(_rpcClient, false);
    }

    public IElectrumXBlockchainClient Blockchain => _blockchain;
    public IElectrumXServerClient Server => _server;
    public IElectrumXMasterNodeClient MasterNode => _masterNode;
    public IElectrumXProTxClient ProTx => _proTx;
    public IElectrumXMempoolClient Mempool => _mempool;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        await _blockchain.DisposeAsync().ConfigureAwait(false);
        await _server.DisposeAsync().ConfigureAwait(false);
        await _masterNode.DisposeAsync().ConfigureAwait(false);
        await _proTx.DisposeAsync().ConfigureAwait(false);
        await _mempool.DisposeAsync().ConfigureAwait(false);
        if (_ownsRpcClient)
        {
            await _rpcClient.DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public ElectrumXServerVersion ServerVersion => _rpcClient.ServerVersion;

    /// <summary>Creates and connects a client using unencrypted TCP.</summary>
    /// <param name="host">The server host name or IP address.</param>
    /// <param name="port">The server port.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected client exposing all supported ElectrumX API groups.</returns>
    public static Task<ElectrumXClient> ConnectAsync(
        string host,
        int port,
        CancellationToken cancellationToken = default)
    {
        return ConnectAsync(
            new ElectrumXTcpRpcClient(new ElectrumXRpcClientOptions
            {
                Host = host, Port = port
            }),
            cancellationToken: cancellationToken);
    }

    /// <summary>Connects using a configured JSON-RPC client implementation.</summary>
    /// <param name="rpcClient">The JSON-RPC transport to connect and use; must not be <see langword="null" />.</param>
    /// <param name="ownsRpcClient">Whether disposing the returned client also disposes <paramref name="rpcClient" />; defaults to <see langword="true" />.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected client exposing all supported ElectrumX API groups.</returns>
    public static async Task<ElectrumXClient> ConnectAsync(
        IElectrumXRpcClient rpcClient,
        bool ownsRpcClient = true,
        CancellationToken cancellationToken = default)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(rpcClient, nameof(rpcClient));

        var client = new ElectrumXClient(rpcClient, ownsRpcClient);

        try
        {
            if (!rpcClient.IsConnected)
            {
                await rpcClient.ConnectAsync(cancellationToken).ConfigureAwait(false);
            }

            return client;
        }
        catch
        {
            await client.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }
}
