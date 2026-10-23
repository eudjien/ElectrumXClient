namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>An unspent transaction output associated with a script hash.</summary>
public sealed class ElectrumXUnspentOutput : ElectrumXResult
{
    /// <summary>Creates an unspent output entry returned for a script hash.</summary>
    public ElectrumXUnspentOutput(
        long height,
        int transactionPosition,
        string transactionHash,
        long value,
        ElectrumXTokenData? tokenData = null)
    {
        Height = height;
        TransactionPosition = transactionPosition;
        TransactionHash = transactionHash;
        Value = value;
        TokenData = tokenData;
    }

    /// <summary>Confirmation height, or <c>0</c> for an unconfirmed transaction.</summary>
    public long Height { get; }

    /// <summary>Zero-based transaction position in the block when provided by the server.</summary>
    public int TransactionPosition { get; }

    /// <summary>32-byte transaction hash containing this output, encoded as 64 hexadecimal characters.</summary>
    public string TransactionHash { get; }

    /// <summary>Output amount in minimum coin units (satoshis on Bitcoin).</summary>
    public long Value { get; }

    /// <summary>Optional CashToken metadata when this output contains tokens.</summary>
    public ElectrumXTokenData? TokenData { get; }
}
