using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.get_tsc_merkle containing serialized transaction data.</summary>
public sealed class ElectrumXTscMerkleWithTransactionHexQuery : ElectrumXQuery
{
    public ElectrumXTscMerkleWithTransactionHexQuery(
        string transactionHash,
        long height,
        TscMerkleTargetType targetType =
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXTscMerkleWithTransactionHexQuery.TargetType)
    {
        if (!Enum.IsDefined(typeof(TscMerkleTargetType), targetType))
        {
            throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
        }

        TransactionHash = ArgumentGuard.ValidateAndNormalizeSha256Hash(transactionHash, nameof(transactionHash));
        Height = height;
        TargetType = targetType;
    }

    public string TransactionHash { get; }

    public long Height { get; }

    public TscMerkleTargetType TargetType { get; }
}
