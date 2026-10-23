using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.get_merkle.</summary>
public sealed class ElectrumXTransactionMerkleQuery : ElectrumXQuery
{
    public ElectrumXTransactionMerkleQuery(string transactionHash, long? height = null)
    {
        TransactionHash = ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
        if (height is < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        Height = height;
    }

    public string TransactionHash { get; }

    public long? Height { get; }
}
