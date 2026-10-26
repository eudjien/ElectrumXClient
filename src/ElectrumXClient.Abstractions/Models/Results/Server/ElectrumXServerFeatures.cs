using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>Features and services advertised by an ElectrumX server.</summary>
public sealed class ElectrumXServerFeatures : ElectrumXResult
{
    /// <summary>Creates a server feature result, retaining fields unknown to this protocol model.</summary>
    /// <param name="hosts">Advertised TCP and WebSocket host/port combinations.</param>
    /// <param name="genesisHash">Hash of the genesis block for this blockchain.</param>
    /// <param name="hashFunction">Name of the hash function used by the server.</param>
    /// <param name="serverVersion">Software version string advertised by the server.</param>
    /// <param name="protocolMinimum">Lowest Electrum protocol version supported by the server.</param>
    /// <param name="protocolMaximum">Highest Electrum protocol version supported by the server.</param>
    /// <param name="pruning">Pruning limit in blocks, or <see langword="null" /> if the server is not pruned.</param>
    /// <param name="additionalProperties">Additional server fields, or <see langword="null" /> when none were returned.</param>
    public ElectrumXServerFeatures(IReadOnlyDictionary<string, ElectrumXServerHostPorts> hosts, string genesisHash, string hashFunction,
        string serverVersion,
        string protocolMinimum, string protocolMaximum, int? pruning,
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null)
    {
        Hosts = hosts;
        GenesisHash = genesisHash;
        HashFunction = hashFunction;
        ServerVersion = serverVersion;
        ProtocolMinimum = protocolMinimum;
        ProtocolMaximum = protocolMaximum;
        Pruning = pruning;

        var additionalPropertiesCopy = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);

        if (additionalProperties is not null)
        {
            foreach ((string name, var value) in additionalProperties)
            {
                additionalPropertiesCopy[name] = value;
            }
        }

        AdditionalProperties = new ReadOnlyDictionary<string, ElectrumXJsonValue>(additionalPropertiesCopy);
    }

    /// <summary>Advertised TCP and WebSocket host/port combinations.</summary>
    public IReadOnlyDictionary<string, ElectrumXServerHostPorts> Hosts { get; }

    /// <summary>32-byte genesis block hash encoded as 64 hexadecimal characters.</summary>
    public string GenesisHash { get; }

    /// <summary>Name of the hash function used by the server.</summary>
    public string HashFunction { get; }

    /// <summary>Software version string advertised by the server.</summary>
    public string ServerVersion { get; }

    /// <summary>Lowest Electrum protocol version supported by the server, formatted as a dotted version string.</summary>
    public string ProtocolMinimum { get; }

    /// <summary>Highest Electrum protocol version supported by the server, formatted as a dotted version string.</summary>
    public string ProtocolMaximum { get; }

    /// <summary>Pruning limit in blocks, or <see langword="null" /> if the server is not pruned.</summary>
    public int? Pruning { get; }

    /// <summary>Additional server feature fields not defined by the current protocol model.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; }
}
