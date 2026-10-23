using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Clients;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.MasterNode;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using NorthGomel.ElectrumXClient.JsonRpc;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ElectrumXRpcClientOptions=NorthGomel.ElectrumXClient.JsonRpc.ElectrumXRpcClientOptions;

namespace NorthGomel.ElectrumXClient.Clients;

/// <inheritdoc />
public sealed class ElectrumXMasterNodeClient : ElectrumXClientBase, IElectrumXMasterNodeClient
{
    internal ElectrumXMasterNodeClient(IElectrumXRpcClient rpcClient, bool ownsRpcClient = true)
        : base(rpcClient, ownsRpcClient)
    {
        RpcClient.MasterNodeStatusChanged += HandleMasterNodeStatus;
    }

    /// <inheritdoc />
    public override ElectrumXServerVersion ServerVersion => RpcClient.ServerVersion;

    /// <inheritdoc />
    public event EventHandler<ElectrumXMasterNodeStatusEventArgs>? StatusChanged;

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        RpcClient.MasterNodeStatusChanged -= HandleMasterNodeStatus;
        await base.DisposeAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBroadcastMasterNodeAnnouncement> BroadcastAnnouncementAsync(
        string signedAnnouncement,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return await CallOrThrowAsync<ElectrumXBroadcastMasterNodeAnnouncement, ElectrumXBroadcastMasterNodeAnnouncementQuery>(
            new JsonRpcRequest<ElectrumXBroadcastMasterNodeAnnouncementQuery>(
                ElectrumXJsonMethodNames.MasterNode.AnnouncementBroadcast,
                new ElectrumXBroadcastMasterNodeAnnouncementQuery(signedAnnouncement)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXMasterNodeSubscription> SubscribeToMasterNodeAsync(
        string collateral,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return await CallOrThrowAsync<ElectrumXMasterNodeSubscription, ElectrumXSubscribeToMasterNodeQuery>(
            new JsonRpcRequest<ElectrumXSubscribeToMasterNodeQuery>(
                ElectrumXJsonMethodNames.MasterNode.Subscribe,
                new ElectrumXSubscribeToMasterNodeQuery(collateral)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXMasterNodes> GetMasterNodesAsync(
        IReadOnlyCollection<string> payees,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        return await CallOrThrowAsync<ElectrumXMasterNodes, ElectrumXMasterNodesQuery>(
            new JsonRpcRequest<ElectrumXMasterNodesQuery>(
                ElectrumXJsonMethodNames.MasterNode.List,
                new ElectrumXMasterNodesQuery(payees)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Connects a masternode API client using the supplied JSON-RPC transport.</summary>
    /// <param name="rpcClient">The JSON-RPC transport to connect and use; must not be <see langword="null" />.</param>
    /// <param name="ownsRpcClient">Whether disposing this client also disposes <paramref name="rpcClient" />; defaults to <see langword="true" />.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected masternode API client.</returns>
    public static async Task<ElectrumXMasterNodeClient> ConnectAsync(
        IElectrumXRpcClient rpcClient,
        bool ownsRpcClient = true,
        CancellationToken cancellationToken = default)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(rpcClient, nameof(rpcClient));
        var client = new ElectrumXMasterNodeClient(rpcClient, ownsRpcClient);

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

    /// <summary>Creates and connects a masternode API client using unencrypted TCP.</summary>
    /// <param name="host">The server host name or IP address.</param>
    /// <param name="port">The server port.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected masternode API client.</returns>
    public static Task<ElectrumXMasterNodeClient> ConnectAsync(
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

    private void HandleMasterNodeStatus(object? sender, ElectrumXMasterNodeStatusEventArgs eventArgs)
    {
        ElectrumXNotificationPublisher.Publish(StatusChanged, this, eventArgs);
    }
}
