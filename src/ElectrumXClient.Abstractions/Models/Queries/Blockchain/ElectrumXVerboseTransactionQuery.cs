namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.get with verbose=true.</summary>
public sealed class ElectrumXVerboseTransactionQuery(string transactionHash) : ElectrumXQuery
{
    public string TransactionHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
}
