namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;

/// <summary>Indicates whether a master-node announcement was accepted.</summary>
public sealed class ElectrumXBroadcastMasterNodeAnnouncement : ElectrumXResult
{
    /// <summary>Creates a result indicating whether the masternode announcement was broadcast.</summary>
    public ElectrumXBroadcastMasterNodeAnnouncement(bool value)
    {
        Value = value;
    }

    /// <summary>Whether the server reported that the announcement was broadcast successfully.</summary>
    public bool Value { get; }

    /// <summary>Converts the response to the server's broadcast result.</summary>
    /// <param name="response">The masternode announcement broadcast response.</param>
    /// <returns><see langword="true" /> if the server accepted the announcement.</returns>
    public static implicit operator bool(ElectrumXBroadcastMasterNodeAnnouncement response)
    {
        return response.Value;
    }
}
