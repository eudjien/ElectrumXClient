namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.utxo.get_address.</summary>
public sealed class ElectrumXUtxoAddressQuery : ElectrumXQuery
{
    public ElectrumXUtxoAddressQuery(string transactionHash, int outputIndex)
    {
        ArgumentGuard.ThrowIfNull(transactionHash, nameof(transactionHash));
        ArgumentGuard.ThrowIfNegative(outputIndex, nameof(outputIndex));
        TransactionHash = transactionHash;
        OutputIndex = outputIndex;
    }

    public string TransactionHash { get; }

    public int OutputIndex { get; }
}
