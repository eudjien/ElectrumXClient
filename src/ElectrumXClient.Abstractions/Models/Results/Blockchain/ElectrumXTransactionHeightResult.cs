namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A transaction confirmation height returned by the transaction-height RPC method.</summary>
public sealed class ElectrumXTransactionHeightResult(long? height) : ElectrumXResult
{
    /// <summary>The confirmation height, or <see langword="null" /> when the transaction is unknown.</summary>
    public long? Height { get; } = height;
}
