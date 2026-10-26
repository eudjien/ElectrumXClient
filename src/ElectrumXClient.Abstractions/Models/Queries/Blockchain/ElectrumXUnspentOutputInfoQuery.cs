using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.utxo.get_info.</summary>
public sealed class ElectrumXUnspentOutputInfoQuery : ElectrumXQuery
{
    /// <summary>Creates a query for metadata about a transaction output.</summary>
    /// <param name="transactionHash">Hash of the transaction containing the output.</param>
    /// <param name="outputIndex">Zero-based index of the output.</param>
    public ElectrumXUnspentOutputInfoQuery(string transactionHash, int outputIndex)
    {
        if (outputIndex is < 0 or > 65535)
        {
            throw new ArgumentOutOfRangeException(nameof(outputIndex), outputIndex, "Output index must be between 0 and 65535.");
        }

        TransactionHash = ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
        OutputIndex = outputIndex;
    }

    /// <summary>Gets the hash of the transaction containing the output.</summary>
    public string TransactionHash { get; }

    /// <summary>Gets the zero-based output index.</summary>
    public int OutputIndex { get; }
}
