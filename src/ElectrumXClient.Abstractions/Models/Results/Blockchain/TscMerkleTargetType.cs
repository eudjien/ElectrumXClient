namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

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
