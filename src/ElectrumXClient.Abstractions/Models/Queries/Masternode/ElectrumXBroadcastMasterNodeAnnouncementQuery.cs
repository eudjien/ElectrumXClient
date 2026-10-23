namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.MasterNode;

/// <summary>Query parameters for masternode.announce.broadcast.</summary>
public sealed class ElectrumXBroadcastMasterNodeAnnouncementQuery(string signedAnnouncement) : ElectrumXQuery
{
    public string SignedAnnouncement { get; } = ArgumentGuard.ValidateAndNormalizeHex(signedAnnouncement, nameof(signedAnnouncement));
}
