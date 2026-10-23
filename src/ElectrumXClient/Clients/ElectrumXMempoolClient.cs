using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Clients;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Mempool;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;
using System.Threading;
using System.Threading.Tasks;
using ElectrumXRpcClientOptions=NorthGomel.ElectrumXClient.JsonRpc.ElectrumXRpcClientOptions;

namespace NorthGomel.ElectrumXClient.Clients;

/// <inheritdoc />
public sealed class ElectrumXMempoolClient : ElectrumXClientBase, IElectrumXMempoolClient
{
    internal ElectrumXMempoolClient(IElectrumXRpcClient rpcClient, bool ownsRpcClient = true)
        : base(rpcClient, ownsRpcClient)
    {
    }

    /// <inheritdoc />
    public override ElectrumXServerVersion ServerVersion => RpcClient.ServerVersion;

    /// <inheritdoc />
    public async Task<ElectrumXFeeHistogram> GetFeeHistogramAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version120, ElectrumXJsonMethodNames.Mempool.GetFeeHistogram);
        return await CallOrThrowAsync<ElectrumXFeeHistogram, ElectrumXFeeHistogramQuery>(
            new JsonRpcRequest<ElectrumXFeeHistogramQuery>(
                ElectrumXJsonMethodNames.Mempool.GetFeeHistogram,
                new ElectrumXFeeHistogramQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXMempoolInfo> GetInfoAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version160, ElectrumXJsonMethodNames.Mempool.GetInfo);

        return await CallOrThrowAsync<ElectrumXMempoolInfo, ElectrumXMempoolInfoQuery>(
            new JsonRpcRequest<ElectrumXMempoolInfoQuery>(
                ElectrumXJsonMethodNames.Mempool.GetInfo,
                new ElectrumXMempoolInfoQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Connects a mempool API client using the supplied JSON-RPC transport.</summary>
    /// <param name="rpcClient">The JSON-RPC transport to connect and use; must not be <see langword="null" />.</param>
    /// <param name="ownsRpcClient">Whether disposing this client also disposes <paramref name="rpcClient" />; defaults to <see langword="true" />.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected mempool API client.</returns>
    public static async Task<ElectrumXMempoolClient> ConnectAsync(
        IElectrumXRpcClient rpcClient,
        bool ownsRpcClient = true,
        CancellationToken cancellationToken = default)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(rpcClient, nameof(rpcClient));

        var client = new ElectrumXMempoolClient(rpcClient, ownsRpcClient);

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

    /// <summary>Creates and connects a mempool API client using unencrypted TCP.</summary>
    /// <param name="host">The server host name or IP address.</param>
    /// <param name="port">The server port.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected mempool API client.</returns>
    public static Task<ElectrumXMempoolClient> ConnectAsync(
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
}
