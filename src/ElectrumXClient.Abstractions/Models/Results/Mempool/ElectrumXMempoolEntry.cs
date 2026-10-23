namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;

/// <summary>An unconfirmed transaction associated with a script hash.</summary>
public sealed class ElectrumXMempoolEntry : ElectrumXResult
{
    /// <summary>Creates an unconfirmed transaction entry from a script-hash mempool response.</summary>
    public ElectrumXMempoolEntry(int height, string transactionHash, long fee)
    {
        Height = height;
        TransactionHash = transactionHash;
        Fee = fee;
    }

    /// <summary><c>0</c> when all inputs are confirmed, or <c>-1</c> otherwise.</summary>
    public int Height { get; }

    /// <summary>32-byte transaction hash encoded as 64 hexadecimal characters.</summary>
    public string TransactionHash { get; }

    /// <summary>Transaction fee in minimum coin units (satoshis on Bitcoin).</summary>
    public long Fee { get; }
}
