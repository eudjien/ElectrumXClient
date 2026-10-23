using NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.Abstractions.Clients;

/// <summary>Provides operations for the masternode-related subset of ElectrumX.</summary>
public interface IElectrumXMasterNodeClient : IAsyncDisposable
{
    /// <summary>Server software and negotiated protocol version.</summary>
    /// <remarks>Negotiated using JSON-RPC method <c>server.version</c>.</remarks>
    ElectrumXServerVersion ServerVersion { get; }

    /// <summary>Occurs when the masternode status changes.</summary>
    event EventHandler<ElectrumXMasterNodeStatusEventArgs>? StatusChanged;

    /// <summary>Passes a signed masternode announcement to the daemon for broadcasting.</summary>
    /// <param name="signedAnnouncement">Non-empty serialized signed announcement encoded as an even number of hexadecimal characters.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The server's broadcast result.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>masternode.announce.broadcast</c>; no Electrum Cash protocol version is specified. Available only on compatible Dash-derived
    ///     ElectrumX servers.
    /// </remarks>
    Task<ElectrumXBroadcastMasterNodeAnnouncement> BroadcastAnnouncementAsync(
        string signedAnnouncement,
        CancellationToken cancellationToken = default);

    /// <summary>Subscribes to status updates for the masternode identified by its collateral.</summary>
    /// <param name="collateral">Non-empty collateral identifier accepted by the Dash-compatible server; this client does not impose a specific textual format.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The current masternode subscription status.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>masternode.subscribe</c>; no Electrum Cash protocol version is specified. Subsequent updates are raised through
    ///     <see cref="StatusChanged" />.
    /// </remarks>
    Task<ElectrumXMasterNodeSubscription> SubscribeToMasterNodeAsync(
        string collateral,
        CancellationToken cancellationToken = default);

    /// <summary>Returns masternode information for the requested payee addresses.</summary>
    /// <param name="payees">
    ///     Non-null collection of non-empty payee addresses. The server defines the accepted address format and collection-size limits; an empty
    ///     collection is allowed by this client.
    /// </param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Information for the requested payees.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>masternode.list</c>; no Electrum Cash protocol version is specified. Available only on compatible Dash-derived ElectrumX
    ///     servers.
    /// </remarks>
    Task<ElectrumXMasterNodes> GetMasterNodesAsync(
        IReadOnlyCollection<string> payees,
        CancellationToken cancellationToken = default);
}
