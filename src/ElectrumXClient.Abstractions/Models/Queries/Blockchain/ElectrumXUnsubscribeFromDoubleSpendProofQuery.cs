namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for unsubscribing from double-spend-proof updates.</summary>
public sealed class ElectrumXUnsubscribeFromDoubleSpendProofQuery(string transactionHash)
    : ElectrumXQuery
{
    /// <summary>Gets the hash of the transaction whose updates should no longer be monitored.</summary>
    public string TransactionHash { get; } =
        ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
}
