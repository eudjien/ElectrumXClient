using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>Server identity and negotiated Electrum protocol version.</summary>
public sealed class ElectrumXServerVersion : ElectrumXResult
{
    /// <summary>Creates a result containing the server software and negotiated protocol versions.</summary>
    public ElectrumXServerVersion(string softwareVersion, ElectrumXProtocolVersion protocolVersion)
    {
        SoftwareVersion = softwareVersion;
        ProtocolVersion = protocolVersion ?? throw new ArgumentNullException(nameof(protocolVersion));
    }

    /// <summary>Server software version string returned by <c>server.version</c>.</summary>
    public string SoftwareVersion { get; }

    /// <summary>Negotiated Electrum protocol version.</summary>
    public ElectrumXProtocolVersion ProtocolVersion { get; }
}
