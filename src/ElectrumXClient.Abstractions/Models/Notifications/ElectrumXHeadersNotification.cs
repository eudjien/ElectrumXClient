namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a blockchain.headers.subscribe notification.</summary>
public sealed class ElectrumXHeadersNotification(
    string hex,
    int height,
    ElectrumXJsonValue? legacyHeader = null) : ElectrumXNotification
{
    public string Hex { get; } = hex;

    public int Height { get; } = height;

    /// <summary>Original coin-specific deserialized header when the negotiated protocol uses legacy header objects.</summary>
    public ElectrumXJsonValue? LegacyHeader { get; } = legacyHeader;
}
