namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for subscribing to transaction status updates.</summary>
public sealed class ElectrumXSubscribeToTransactionQuery(string transactionHash)
    : ElectrumXQuery
{
    /// <summary>Gets the hash of the transaction to monitor.</summary>
    public string TransactionHash { get; } =
        ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
}
