namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for retrieving a transaction's height.</summary>
public sealed class ElectrumXTransactionHeightQuery(string transactionHash)
    : ElectrumXQuery
{
    /// <summary>Gets the hash of the transaction.</summary>
    public string TransactionHash { get; } =
        ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
}
