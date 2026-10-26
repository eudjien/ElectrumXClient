using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.get_merkle.</summary>
public sealed class ElectrumXTransactionMerkleQuery : ElectrumXQuery
{
    /// <summary>Creates a query for a transaction's Merkle proof.</summary>
    /// <param name="transactionHash">Hash of the transaction.</param>
    /// <param name="height">Optional block height; when omitted, the server locates the transaction.</param>
    public ElectrumXTransactionMerkleQuery(string transactionHash, long? height = null)
    {
        TransactionHash = ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
        if (height is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        Height = height;
    }

    /// <summary>Gets the hash of the transaction whose Merkle proof is requested.</summary>
    public string TransactionHash { get; }

    /// <summary>Gets the optional block height used to locate the transaction.</summary>
    public long? Height { get; }
}
