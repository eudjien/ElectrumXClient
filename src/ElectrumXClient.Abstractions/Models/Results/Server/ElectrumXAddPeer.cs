namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>Indicates whether the server accepted a peer advertisement.</summary>
/// <param name="value">Whether the server tentatively accepted the peer advertisement.</param>
public sealed class ElectrumXAddPeer(bool value) : ElectrumXResult
{
    /// <summary>Whether the peer advertisement was tentatively accepted.</summary>
    public bool Value { get; } = value;

    public static implicit operator bool(ElectrumXAddPeer response)
    {
        return response.Value;
    }
}
