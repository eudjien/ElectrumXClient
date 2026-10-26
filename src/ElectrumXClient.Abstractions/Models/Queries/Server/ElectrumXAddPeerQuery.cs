using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;

/// <summary>Query parameters for server.add_peer.</summary>
public sealed class ElectrumXAddPeerQuery : ElectrumXQuery
{
    /// <summary>Creates a query that advertises this server's features to a peer.</summary>
    /// <param name="hosts">Transport host names and ports supported by the server.</param>
    /// <param name="genesisHash">Hash of the server's genesis block.</param>
    /// <param name="hashFunction">Hash function used by the server; currently <c>sha256</c>.</param>
    /// <param name="serverVersion">Server software version.</param>
    /// <param name="protocolMinimum">Minimum supported protocol version.</param>
    /// <param name="protocolMaximum">Maximum supported protocol version.</param>
    /// <param name="pruning">Optional pruning depth.</param>
    /// <param name="additionalProperties">Optional feature values not represented by the typed properties.</param>
    public ElectrumXAddPeerQuery(
        IReadOnlyDictionary<string, ElectrumXServerHostPorts> hosts,
        string genesisHash,
        string hashFunction,
        string serverVersion,
        string protocolMinimum,
        string protocolMaximum,
        int? pruning = null,
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null)
    {
        ArgumentGuard.ThrowIfNull(hosts, nameof(hosts));
        ArgumentGuard.ThrowIfNullOrWhiteSpace(genesisHash, nameof(genesisHash));
        ArgumentGuard.ThrowIfNullOrWhiteSpace(hashFunction, nameof(hashFunction));
        ArgumentGuard.ThrowIfNullOrWhiteSpace(serverVersion, nameof(serverVersion));
        ArgumentGuard.ThrowIfNullOrWhiteSpace(protocolMinimum, nameof(protocolMinimum));
        ArgumentGuard.ThrowIfNullOrWhiteSpace(protocolMaximum, nameof(protocolMaximum));

        if (hashFunction != ElectrumXJsonPropertyValues.ServerFeatures.HashFunctionSha256)
        {
            throw new ArgumentException(
                "The only supported server feature hash function is 'sha256'.",
                nameof(hashFunction));
        }

        Hosts = hosts;
        GenesisHash = genesisHash;
        HashFunction = hashFunction;
        ServerVersion = serverVersion;
        ProtocolMinimum = protocolMinimum;
        ProtocolMaximum = protocolMaximum;
        Pruning = pruning;
        AdditionalProperties = additionalProperties;
    }

    /// <summary>Gets the supported transport host names and ports.</summary>
    public IReadOnlyDictionary<string, ElectrumXServerHostPorts> Hosts { get; }

    /// <summary>Gets the hash of the server's genesis block.</summary>
    public string GenesisHash { get; }

    /// <summary>Gets the server's hash function.</summary>
    public string HashFunction { get; }

    /// <summary>Gets the server software version.</summary>
    public string ServerVersion { get; }

    /// <summary>Gets the minimum supported protocol version.</summary>
    public string ProtocolMinimum { get; }

    /// <summary>Gets the maximum supported protocol version.</summary>
    public string ProtocolMaximum { get; }

    /// <summary>Gets the optional pruning depth.</summary>
    public int? Pruning { get; }

    /// <summary>Gets additional feature values not represented by typed properties.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue>? AdditionalProperties { get; }
}
