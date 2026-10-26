namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The result returned by blockchain.headers.get_tip.</summary>
public sealed class ElectrumXHeaderTip(long height, string hex) : ElectrumXResult
{
    /// <summary>Tip height.</summary>
    public long Height { get; } = height;

    /// <summary>Raw 80-byte block header encoded as 160 hexadecimal characters.</summary>
    public string Hex { get; } = hex;
}
