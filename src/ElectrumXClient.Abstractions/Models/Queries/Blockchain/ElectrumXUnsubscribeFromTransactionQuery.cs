namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for unsubscribing from transaction status updates.</summary>
public sealed class ElectrumXUnsubscribeFromTransactionQuery(string transactionHash)
    : ElectrumXQuery
{
    /// <summary>Gets the hash of the transaction to stop monitoring.</summary>
    public string TransactionHash { get; } =
        ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
}
