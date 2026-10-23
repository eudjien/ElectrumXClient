namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A transaction in a script hash's confirmed or mempool history.</summary>
public sealed class ElectrumXHistoryEntry : ElectrumXResult
{
    /// <summary>Creates a confirmed or mempool history entry.</summary>
    public ElectrumXHistoryEntry(long height, string transactionHash, long? fee = null)
    {
        Height = height;
        TransactionHash = transactionHash;
        Fee = fee;
    }

    /// <summary>Block height, or the server's unconfirmed height value for a mempool transaction.</summary>
    public long Height { get; }

    /// <summary>32-byte transaction hash encoded as 64 hexadecimal characters.</summary>
    public string TransactionHash { get; }

    /// <summary>Mempool transaction fee in minimum coin units (satoshis on Bitcoin); omitted for confirmed entries.</summary>
    public long? Fee { get; }
}
