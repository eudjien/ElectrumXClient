namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.get with verbose=false.</summary>
public sealed class ElectrumXRawTransactionQuery(string transactionHash) : ElectrumXQuery
{
    /// <summary>Gets the hash of the transaction to retrieve.</summary>
    public string TransactionHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
}
