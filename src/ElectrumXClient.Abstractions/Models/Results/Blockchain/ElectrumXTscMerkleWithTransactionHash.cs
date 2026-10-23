using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>TSC Merkle proof containing the transaction hash.</summary>
public sealed class ElectrumXTscMerkleWithTransactionHash : ElectrumXTscMerkle
{
    /// <summary>Creates a TSC Merkle proof that identifies its transaction by hash.</summary>
    public ElectrumXTscMerkleWithTransactionHash(bool composite, int index, IReadOnlyList<string> nodes, string proofType, string target,
        TscMerkleTargetType targetType, string transactionHash)
        : base(composite, index, nodes, proofType, target, targetType)
    {
        TransactionHash = transactionHash;
    }

    /// <summary>32-byte transaction hash proved to be included in the block, encoded as 64 hexadecimal characters.</summary>
    public string TransactionHash { get; }
}
