using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>The peers returned by server.peers.subscribe.</summary>
public sealed class ElectrumXPeers : ElectrumXResult
{
    /// <summary>Creates a result containing the peer list.</summary>
    public ElectrumXPeers(IReadOnlyList<ElectrumXPeer> value)
    {
        Value = value;
    }

    /// <summary>Peer servers returned by <c>server.peers.subscribe</c>.</summary>
    public IReadOnlyList<ElectrumXPeer> Value { get; }

    /// <summary>Converts the response to its peer entries.</summary>
    /// <param name="response">The server peers response.</param>
    /// <returns>The contained peers.</returns>
    public static implicit operator ElectrumXPeer[](ElectrumXPeers response)
    {
        return [.. response.Value];
    }
}

/// <summary>A peer returned by server.peers.subscribe.</summary>
public sealed class ElectrumXPeer : ElectrumXResult
{
    /// <summary>Creates a peer entry returned by <c>server.peers.subscribe</c>.</summary>
    public ElectrumXPeer(string ipAddress, string hostName, IReadOnlyList<string> features)
    {
        IpAddress = ipAddress;
        HostName = hostName;
        Features = features;
    }

    /// <summary>Peer IP address in textual IPv4 or IPv6 notation.</summary>
    public string IpAddress { get; }

    /// <summary>Peer DNS host name, or an IP address in textual notation.</summary>
    public string HostName { get; }

    /// <summary>Compact peer feature tokens for protocol version (<c>v</c>), pruning limit (<c>p</c>), TCP port (<c>t</c>), and SSL port (<c>s</c>).</summary>
    public IReadOnlyList<string> Features { get; }
}
