using System;

namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>Defines validated settings for an ElectrumX JSON-RPC client.</summary>
public interface IElectrumXRpcClientOptions
{
    /// <summary>Gets the JSON converter used to serialize requests and deserialize results and notifications.</summary>
    IElectrumXJsonConverter Converter { get; init; }

    /// <summary>Gets the validated hostname or IP address of the ElectrumX server.</summary>
    string Host { get; init; }

    /// <summary>Gets the TCP port of the ElectrumX server, from 1 through 65535.</summary>
    int Port { get; init; }

    /// <summary>Gets the client name sent during protocol negotiation.</summary>
    string ClientName { get; init; }

    /// <summary>Gets the minimum and maximum protocol versions supported by this client.</summary>
    ElectrumXProtocolVersionLimits ProtocolVersions { get; init; }

    /// <summary>Gets the positive maximum time allowed to establish and negotiate a connection.</summary>
    TimeSpan ConnectTimeout { get; init; }

    /// <summary>Gets the positive maximum time allowed for an RPC request to complete.</summary>
    TimeSpan RequestTimeout { get; init; }
}
