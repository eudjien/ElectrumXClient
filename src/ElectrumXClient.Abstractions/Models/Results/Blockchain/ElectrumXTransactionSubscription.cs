namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A transaction confirmation height returned by the transaction-subscription RPC method.</summary>
public sealed class ElectrumXTransactionSubscription(long? height) : ElectrumXResult
{
    /// <summary>The current confirmation height, or <see langword="null" /> when the transaction is unknown.</summary>
    public long? Height { get; } = height;
}
