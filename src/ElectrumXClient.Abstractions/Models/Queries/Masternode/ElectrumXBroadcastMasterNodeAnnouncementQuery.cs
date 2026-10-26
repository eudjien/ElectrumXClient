namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Masternode;

/// <summary>Query parameters for masternode.announce.broadcast.</summary>
public sealed class ElectrumXBroadcastMasterNodeAnnouncementQuery(string signedAnnouncement) : ElectrumXQuery
{
    /// <summary>Gets the signed masternode announcement serialized as hexadecimal.</summary>
    public string SignedAnnouncement { get; } = ArgumentGuard.ValidateAndNormalizeHex(signedAnnouncement, nameof(signedAnnouncement));
}
