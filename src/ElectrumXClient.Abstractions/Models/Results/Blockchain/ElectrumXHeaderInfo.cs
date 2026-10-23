namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Block header and its height.</summary>
public sealed class ElectrumXHeaderInfo(long height, string hex) : ElectrumXResult
{
    /// <summary>Block height.</summary>
    public long Height { get; } = height;

    /// <summary>Raw 80-byte block header encoded as 160 hexadecimal characters.</summary>
    public string Hex { get; } = hex;
}
