namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.utxo.get_address.</summary>
public sealed class ElectrumXUtxoAddressQuery : ElectrumXQuery
{
    /// <summary>Creates a query for the address associated with a transaction output.</summary>
    /// <param name="transactionHash">Hash of the transaction containing the output.</param>
    /// <param name="outputIndex">Zero-based index of the output.</param>
    public ElectrumXUtxoAddressQuery(string transactionHash, int outputIndex)
    {
        transactionHash = ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
        ArgumentGuard.ThrowIfNegative(outputIndex, nameof(outputIndex));
        TransactionHash = transactionHash;
        OutputIndex = outputIndex;
    }

    /// <summary>Gets the hash of the transaction containing the output.</summary>
    public string TransactionHash { get; }

    /// <summary>Gets the zero-based output index.</summary>
    public int OutputIndex { get; }
}
