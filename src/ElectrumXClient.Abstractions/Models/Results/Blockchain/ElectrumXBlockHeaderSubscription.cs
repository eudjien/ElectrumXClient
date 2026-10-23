namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A block header and height returned by a header subscription.</summary>
public sealed class ElectrumXBlockHeaderSubscription : ElectrumXResult
{
    /// <summary>Creates a chain-tip header result.</summary>
    public ElectrumXBlockHeaderSubscription(long height, string hex, ElectrumXJsonValue? legacyHeader = null)
    {
        Height = height;
        Hex = hex;
        LegacyHeader = legacyHeader;
    }

    /// <summary>Height of the block header in the main chain.</summary>
    public long Height { get; }

    /// <summary>Raw 80-byte block header encoded as 160 hexadecimal characters.</summary>
    public string Hex { get; }

    /// <summary>
    ///     Original coin-specific deserialized header returned by protocol versions before 1.2, or by versions 1.2-1.3 when raw headers are requested as
    ///     false.
    /// </summary>
    public ElectrumXJsonValue? LegacyHeader { get; }
}
