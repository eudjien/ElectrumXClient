using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.Abstractions.Clients;

/// <summary>Provides operations for the server-related subset of ElectrumX.</summary>
public interface IElectrumXServerClient : IAsyncDisposable
{
    /// <summary>Server software and negotiated protocol version.</summary>
    /// <remarks>Negotiated using JSON-RPC method <c>server.version</c>.</remarks>
    ElectrumXServerVersion ServerVersion { get; }

    /// <summary>Submits this server's advertised features to the server as a peer candidate.</summary>
    /// <param name="hosts">TCP and WebSocket host/port combinations advertised by the server.</param>
    /// <param name="genesisHash">Hash of the genesis block for this blockchain.</param>
    /// <param name="hashFunction">Hash function used by the server; currently <c>sha256</c>.</param>
    /// <param name="serverVersion">Software version string advertised by the server.</param>
    /// <param name="protocolMinimum">Lowest Electrum protocol version supported by the server.</param>
    /// <param name="protocolMaximum">Highest Electrum protocol version supported by the server.</param>
    /// <param name="pruning">Pruning limit in blocks, or <see langword="null" /> if the server is not pruned.</param>
    /// <param name="additionalProperties">Additional server feature properties.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Whether the request was tentatively accepted; successful peer publication may follow later sanity checks.</returns>
    /// <remarks>JSON-RPC method: <c>server.add_peer</c>, available since protocol version 1.1. Intended for server software, not ordinary wallet clients.</remarks>
    Task<ElectrumXAddPeer> AddPeerAsync(
        IReadOnlyDictionary<string, ElectrumXServerHostPorts> hosts,
        string genesisHash,
        string hashFunction,
        string serverVersion,
        string protocolMinimum,
        string protocolMaximum,
        int? pruning = null,
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null,
        CancellationToken cancellationToken = default);

    /// <summary>Returns the banner text configured on the server.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The server banner string.</returns>
    /// <remarks>JSON-RPC method: <c>server.banner</c>, available since protocol version 1.0.</remarks>
    Task<ElectrumXBanner> GetBannerAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns the donation address configured on the server.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The donation address string.</returns>
    /// <remarks>JSON-RPC method: <c>server.donation_address</c>, available since protocol version 1.0.</remarks>
    Task<ElectrumXDonationAddress> GetDonationAddressAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns the features and services advertised by the server.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Server feature data, including network genesis hash, supported protocol range, available hosts, and script hash function.</returns>
    /// <remarks>JSON-RPC method: <c>server.features</c>, available since protocol version 1.1.</remarks>
    Task<ElectrumXServerFeatures> GetServerFeaturesAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns the peer servers currently known to the server.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Peer addresses and their advertised protocol, pruning, TCP, and SSL features.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>server.peers.subscribe</c>, available since protocol version 1.0. Despite its name, the protocol defines this as a one-time
    ///     request and sends no notifications.
    /// </remarks>
    Task<ElectrumXPeers> GetPeersAsync(CancellationToken cancellationToken = default);

    /// <summary>Pings the server to ensure the connection is alive and keep the session active.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The server's ping response.</returns>
    /// <remarks>JSON-RPC method: <c>server.ping</c>, available since protocol version 1.2.</remarks>
    Task<ElectrumXPing> PingAsync(CancellationToken cancellationToken = default);

    /// <summary>Forwards an RPC call to the backing daemon and returns its raw JSON result.</summary>
    /// <param name="method">Backing daemon RPC method name.</param>
    /// <param name="parameters">Positional arguments forwarded to the daemon.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The daemon's result without imposing a daemon-specific shape.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>daemon.passthrough</c>, available since protocol version 1.5.2. This is a privileged capability: most servers disable it
    ///     because it exposes unrestricted daemon RPC to clients.
    /// </remarks>
    Task<ElectrumXJsonValueResult> PassthroughAsync(
        string method,
        IReadOnlyList<ElectrumXJsonValue>? parameters = null,
        CancellationToken cancellationToken = default);
}
