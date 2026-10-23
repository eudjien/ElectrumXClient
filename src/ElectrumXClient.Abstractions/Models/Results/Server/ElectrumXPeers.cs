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

    public static implicit operator ElectrumXPeer[](ElectrumXPeers response)
    {
        return [.. response.Value];
    }
}
