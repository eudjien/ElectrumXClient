using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.get_tsc_merkle containing serialized transaction data.</summary>
public sealed class ElectrumXTscMerkleWithTransactionHexQuery : ElectrumXQuery
{
    /// <summary>Creates a TSC Merkle proof query using serialized transaction data.</summary>
    /// <param name="transactionHash">Hash of the transaction being proved.</param>
    /// <param name="height">Height of the block containing the transaction.</param>
    /// <param name="targetType">Kind of target against which the proof is verified.</param>
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

    /// <summary>Gets the hash of the transaction being proved.</summary>
    public string TransactionHash { get; }

    /// <summary>Gets the height of the block containing the transaction.</summary>
    public long Height { get; }

    /// <summary>Gets the kind of target against which the proof is verified.</summary>
    public TscMerkleTargetType TargetType { get; }
}
