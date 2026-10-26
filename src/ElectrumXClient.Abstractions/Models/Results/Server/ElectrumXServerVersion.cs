namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>Server identity and negotiated Electrum protocol version.</summary>
public sealed class ElectrumXServerVersion(string softwareVersion, ElectrumXProtocolVersion protocolVersion) : ElectrumXResult
{

    /// <summary>Server software version string returned by <c>server.version</c>.</summary>
    public string SoftwareVersion { get; } = softwareVersion;

    /// <summary>Negotiated Electrum protocol version.</summary>
    public ElectrumXProtocolVersion ProtocolVersion { get; } = protocolVersion;
}
