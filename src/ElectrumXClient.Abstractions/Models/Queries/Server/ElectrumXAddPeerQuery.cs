using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;

/// <summary>Query parameters for server.add_peer.</summary>
public sealed class ElectrumXAddPeerQuery : ElectrumXQuery
{
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

    public IReadOnlyDictionary<string, ElectrumXServerHostPorts> Hosts { get; }

    public string GenesisHash { get; }

    public string HashFunction { get; }

    public string ServerVersion { get; }

    public string ProtocolMinimum { get; }

    public string ProtocolMaximum { get; }

    public int? Pruning { get; }

    public IReadOnlyDictionary<string, ElectrumXJsonValue>? AdditionalProperties { get; }
}
