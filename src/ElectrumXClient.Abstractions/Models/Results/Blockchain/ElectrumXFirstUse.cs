namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>First known use of an address or script hash.</summary>
public sealed class ElectrumXFirstUse(string blockHash, long height, string transactionHash) : ElectrumXResult
{
    /// <summary>32-byte block hash encoded as 64 hexadecimal characters.</summary>
    public string BlockHash { get; } = blockHash;

    /// <summary>Block height containing the first use.</summary>
    public long Height { get; } = height;

    /// <summary>32-byte transaction hash encoded as 64 hexadecimal characters.</summary>
    public string TransactionHash { get; } = transactionHash;
}
