using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>TSC Merkle proof containing the transaction hash.</summary>
public sealed class ElectrumXTscMerkleWithTransactionHash : ElectrumXResult
{
    /// <summary>Creates a TSC Merkle proof that identifies its transaction by hash.</summary>
    public ElectrumXTscMerkleWithTransactionHash(bool composite, int index, IReadOnlyList<string> nodes, string proofType, string target,
        TscMerkleTargetType targetType, string transactionHash)
    {
        Composite = composite;
        Index = index;
        Nodes = nodes;
        ProofType = proofType;
        Target = target;
        TargetType = targetType;
        TransactionHash = transactionHash;
    }

    /// <summary>Whether the proof is a composite proof.</summary>
    public bool Composite { get; }

    /// <summary>Zero-based transaction index in the block.</summary>
    public int Index { get; }

    /// <summary>32-byte hashes forming the Merkle proof branch, each encoded as 64 hexadecimal characters.</summary>
    public IReadOnlyList<string> Nodes { get; }

    /// <summary>Proof format identifier, such as <c>branch</c>.</summary>
    public string ProofType { get; }

    /// <summary>Hexadecimal proof target, interpreted according to <see cref="TargetType" />.</summary>
    public string Target { get; }

    /// <summary>Kind of block data identified by the target.</summary>
    public TscMerkleTargetType TargetType { get; }

    /// <summary>32-byte transaction hash proved to be included in the block, encoded as 64 hexadecimal characters.</summary>
    public string TransactionHash { get; }
}
