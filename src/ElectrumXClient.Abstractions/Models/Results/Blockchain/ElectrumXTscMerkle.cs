using System;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Common fields for a Bitcoin Association TSC-format Merkle proof.</summary>
public abstract class ElectrumXTscMerkle : ElectrumXResult
{
    /// <summary>Creates the common fields of a TSC-format Merkle proof.</summary>
    protected ElectrumXTscMerkle(bool composite, int index, IReadOnlyList<string> nodes, string proofType, string target, TscMerkleTargetType targetType)
    {
        Composite = composite;
        Index = index;
        Nodes = nodes;
        ProofType = proofType;
        Target = target;
        TargetType = targetType;
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
}

/// <summary>Identifies the block data referenced by a TSC Merkle proof target.</summary>
public enum TscMerkleTargetType
{
    /// <summary>The target is a block hash.</summary>
    BlockHash,

    /// <summary>The target is a serialized block header.</summary>
    BlockHeader,

    /// <summary>The target is a Merkle root.</summary>
    MerkleRoot
}

public static class TscMerkleTargetTypeExtensions
{
    public static string ToWireValue(this TscMerkleTargetType targetType)
    {
        return targetType switch
        {
            TscMerkleTargetType.BlockHash => ElectrumXJsonPropertyValues.TscMerkle.TargetType.BlockHash,
            TscMerkleTargetType.BlockHeader => ElectrumXJsonPropertyValues.TscMerkle.TargetType.BlockHeader,
            TscMerkleTargetType.MerkleRoot => ElectrumXJsonPropertyValues.TscMerkle.TargetType.MerkleRoot,
            _ => throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null)
        };
    }

    public static bool TryParse(string value, out TscMerkleTargetType targetType)
    {
        switch (value)
        {
            case ElectrumXJsonPropertyValues.TscMerkle.TargetType.BlockHash:
                targetType = TscMerkleTargetType.BlockHash;
                return true;
            case ElectrumXJsonPropertyValues.TscMerkle.TargetType.BlockHeader:
                targetType = TscMerkleTargetType.BlockHeader;
                return true;
            case ElectrumXJsonPropertyValues.TscMerkle.TargetType.MerkleRoot:
                targetType = TscMerkleTargetType.MerkleRoot;
                return true;
            default:
                targetType = default;
                return false;
        }
    }
}
