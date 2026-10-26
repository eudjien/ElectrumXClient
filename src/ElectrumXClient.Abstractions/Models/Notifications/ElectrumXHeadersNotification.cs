namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a blockchain.headers.subscribe notification.</summary>
public sealed class ElectrumXHeadersNotification(
    string hex,
    int height,
    ElectrumXJsonValue? legacyHeader = null) : ElectrumXNotification
{
    /// <summary>Gets the serialized block header in hexadecimal form.</summary>
    public string Hex { get; } = hex;

    /// <summary>Gets the height of the block containing the header.</summary>
    public int Height { get; } = height;

    /// <summary>Original coin-specific deserialized header when the negotiated protocol uses legacy header objects.</summary>
    public ElectrumXJsonValue? LegacyHeader { get; } = legacyHeader;
}
