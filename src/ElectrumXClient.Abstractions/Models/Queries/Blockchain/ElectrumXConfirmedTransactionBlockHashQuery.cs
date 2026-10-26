namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.get_confirmed_blockhash.</summary>
public sealed class ElectrumXConfirmedTransactionBlockHashQuery(
    string transactionHash,
    bool includeHeader = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXConfirmedTransactionBlockHashQuery.IncludeHeader)
    : ElectrumXQuery
{
    /// <summary>Gets the hash of the confirmed transaction.</summary>
    public string TransactionHash { get; } =
        ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));

    /// <summary>Gets whether the response should include the block header.</summary>
    public bool IncludeHeader { get; } = includeHeader;
}
