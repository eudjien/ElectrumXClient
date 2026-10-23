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

    public static implicit operator bool(ElectrumXBroadcastMasterNodeAnnouncement response)
    {
        return response.Value;
    }
}
