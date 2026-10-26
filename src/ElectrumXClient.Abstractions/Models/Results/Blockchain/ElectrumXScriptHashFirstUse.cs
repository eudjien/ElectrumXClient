namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The first-use result returned by blockchain.scripthash.get_first_use.</summary>
public sealed class ElectrumXScriptHashFirstUse(string blockHash, long height, string transactionHash)
    : ElectrumXResult
{
    /// <summary>32-byte block hash encoded as 64 hexadecimal characters.</summary>
    public string BlockHash { get; } = blockHash;

    /// <summary>Block height containing the first use.</summary>
    public long Height { get; } = height;

    /// <summary>32-byte transaction hash encoded as 64 hexadecimal characters.</summary>
    public string TransactionHash { get; } = transactionHash;
}
