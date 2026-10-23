using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Clients;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.ProTx;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;
using System.Threading;
using System.Threading.Tasks;
using ElectrumXRpcClientOptions=NorthGomel.ElectrumXClient.JsonRpc.ElectrumXRpcClientOptions;

namespace NorthGomel.ElectrumXClient.Clients;

/// <inheritdoc />
public sealed class ElectrumXProTxClient : ElectrumXClientBase, IElectrumXProTxClient
{
    internal ElectrumXProTxClient(IElectrumXRpcClient rpcClient, bool ownsRpcClient = true)
        : base(rpcClient, ownsRpcClient)
    {
    }

    /// <inheritdoc />
    public override ElectrumXServerVersion ServerVersion => RpcClient.ServerVersion;

    /// <inheritdoc />
    public async Task<ElectrumXProTxDiff> GetDiffAsync(long baseHeight, long height, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return await CallOrThrowAsync<ElectrumXProTxDiff, ElectrumXProTxDiffQuery>(
            new JsonRpcRequest<ElectrumXProTxDiffQuery>(
                ElectrumXJsonMethodNames.ProTx.Diff,
                new ElectrumXProTxDiffQuery(baseHeight, height)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXProTxInfo> GetInfoAsync(string proTxHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return await CallOrThrowAsync<ElectrumXProTxInfo, ElectrumXProTxInfoQuery>(
            new JsonRpcRequest<ElectrumXProTxInfoQuery>(
                ElectrumXJsonMethodNames.ProTx.Info,
                new ElectrumXProTxInfoQuery(proTxHash)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Connects a ProTx API client using the supplied JSON-RPC transport.</summary>
    /// <param name="rpcClient">The JSON-RPC transport to connect and use; must not be <see langword="null" />.</param>
    /// <param name="ownsRpcClient">Whether disposing this client also disposes <paramref name="rpcClient" />; defaults to <see langword="true" />.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected ProTx API client.</returns>
    public static async Task<ElectrumXProTxClient> ConnectAsync(
        IElectrumXRpcClient rpcClient,
        bool ownsRpcClient = true,
        CancellationToken cancellationToken = default)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(rpcClient, nameof(rpcClient));
        var client = new ElectrumXProTxClient(rpcClient, ownsRpcClient);

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

    /// <summary>Creates and connects a ProTx API client using unencrypted TCP.</summary>
    /// <param name="host">The server host name or IP address.</param>
    /// <param name="port">The server port.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected ProTx API client.</returns>
    public static Task<ElectrumXProTxClient> ConnectAsync(
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
