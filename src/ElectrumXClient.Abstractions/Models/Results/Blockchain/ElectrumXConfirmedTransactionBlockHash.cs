namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Block containing a confirmed transaction, optionally including the block header.</summary>
public sealed class ElectrumXConfirmedTransactionBlockHash(
    string blockHash,
    long blockHeight,
    string? blockHeader) : ElectrumXResult
{
    /// <summary>32-byte block hash encoded as 64 hexadecimal characters.</summary>
    public string BlockHash { get; } = blockHash;

    /// <summary>Height of the containing block.</summary>
    public long BlockHeight { get; } = blockHeight;

    /// <summary>Optional raw 80-byte block header encoded as 160 hexadecimal characters.</summary>
    public string? BlockHeader { get; } = blockHeader;
}
