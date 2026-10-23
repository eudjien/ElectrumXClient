using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Clients;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ElectrumXRpcClientOptions=NorthGomel.ElectrumXClient.JsonRpc.ElectrumXRpcClientOptions;

namespace NorthGomel.ElectrumXClient.Clients;

/// <inheritdoc />
public sealed class ElectrumXServerClient : ElectrumXClientBase, IElectrumXServerClient
{
    internal ElectrumXServerClient(IElectrumXRpcClient rpcClient, bool ownsRpcClient = true)
        : base(rpcClient, ownsRpcClient)
    {
    }

    /// <inheritdoc />
    public override ElectrumXServerVersion ServerVersion => RpcClient.ServerVersion;

    /// <inheritdoc />
    public async Task<ElectrumXAddPeer> AddPeerAsync(
        IReadOnlyDictionary<string, ElectrumXServerHostPorts> hosts,
        string genesisHash,
        string hashFunction,
        string serverVersion,
        string protocolMinimum,
        string protocolMaximum,
        int? pruning = null,
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version110, ElectrumXJsonMethodNames.Server.AddPeer);

        return await CallOrThrowAsync<ElectrumXAddPeer, ElectrumXAddPeerQuery>(
                new JsonRpcRequest<ElectrumXAddPeerQuery>(
                    ElectrumXJsonMethodNames.Server.AddPeer,
                    new ElectrumXAddPeerQuery(
                        hosts,
                        genesisHash,
                        hashFunction,
                        serverVersion,
                        protocolMinimum,
                        protocolMaximum,
                        pruning,
                        additionalProperties)),
                cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBanner> GetBannerAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Server.Banner);
        return await CallOrThrowAsync<ElectrumXBanner, ElectrumXBannerQuery>(
            new JsonRpcRequest<ElectrumXBannerQuery>(
                ElectrumXJsonMethodNames.Server.Banner,
                new ElectrumXBannerQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXDonationAddress> GetDonationAddressAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Server.DonationAddress);
        return await CallOrThrowAsync<ElectrumXDonationAddress, ElectrumXDonationAddressQuery>(
            new JsonRpcRequest<ElectrumXDonationAddressQuery>(
                ElectrumXJsonMethodNames.Server.DonationAddress,
                new ElectrumXDonationAddressQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXServerFeatures> GetServerFeaturesAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version110, ElectrumXJsonMethodNames.Server.Features);
        return await CallOrThrowAsync<ElectrumXServerFeatures, ElectrumXServerFeaturesQuery>(
            new JsonRpcRequest<ElectrumXServerFeaturesQuery>(
                ElectrumXJsonMethodNames.Server.Features,
                new ElectrumXServerFeaturesQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXPeers> GetPeersAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Server.PeersSubscribe);
        return await CallOrThrowAsync<ElectrumXPeers, ElectrumXPeersQuery>(
            new JsonRpcRequest<ElectrumXPeersQuery>(
                ElectrumXJsonMethodNames.Server.PeersSubscribe,
                new ElectrumXPeersQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXPing> PingAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version120, ElectrumXJsonMethodNames.Server.Ping);
        return await CallOrThrowAsync<ElectrumXPing, ElectrumXPingQuery>(
            new JsonRpcRequest<ElectrumXPingQuery>(
                ElectrumXJsonMethodNames.Server.Ping,
                new ElectrumXPingQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXJsonValueResult> PassthroughAsync(
        string method,
        IReadOnlyList<ElectrumXJsonValue>? parameters = null,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version152, ElectrumXJsonMethodNames.Daemon.Passthrough);

        return await CallOrThrowAsync<ElectrumXJsonValueResult, ElectrumXDaemonPassthroughQuery>(
            new JsonRpcRequest<ElectrumXDaemonPassthroughQuery>(
                ElectrumXJsonMethodNames.Daemon.Passthrough,
                new ElectrumXDaemonPassthroughQuery(method, parameters)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Connects a server API client using the supplied JSON-RPC transport.</summary>
    /// <param name="rpcClient">The JSON-RPC transport to connect and use; must not be <see langword="null" />.</param>
    /// <param name="ownsRpcClient">Whether disposing this client also disposes <paramref name="rpcClient" />; defaults to <see langword="true" />.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected server API client.</returns>
    public static async Task<ElectrumXServerClient> ConnectAsync(
        IElectrumXRpcClient rpcClient,
        bool ownsRpcClient = true,
        CancellationToken cancellationToken = default)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(rpcClient, nameof(rpcClient));
        var client = new ElectrumXServerClient(rpcClient, ownsRpcClient);

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

    /// <summary>Creates and connects a server API client using unencrypted TCP.</summary>
    /// <param name="host">The server host name or IP address.</param>
    /// <param name="port">The server port.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected server API client.</returns>
    public static Task<ElectrumXServerClient> ConnectAsync(
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
