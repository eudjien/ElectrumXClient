namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>Indicates whether the server accepted a peer advertisement.</summary>
/// <param name="value">Whether the server tentatively accepted the peer advertisement.</param>
public sealed class ElectrumXAddPeer(bool value) : ElectrumXResult
{
    /// <summary>Whether the peer advertisement was tentatively accepted.</summary>
    public bool Value { get; } = value;

    /// <summary>Converts the response to the server's peer-add result.</summary>
    /// <param name="response">The add-peer response.</param>
    /// <returns><see langword="true" /> if the peer was accepted.</returns>
    public static implicit operator bool(ElectrumXAddPeer response)
    {
        return response.Value;
    }
}
