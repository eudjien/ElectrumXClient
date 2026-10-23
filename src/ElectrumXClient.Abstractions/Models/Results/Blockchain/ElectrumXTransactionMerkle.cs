using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Merkle branch proving a transaction's inclusion in a block.</summary>
public sealed class ElectrumXTransactionMerkle : ElectrumXResult
{
    /// <summary>Creates a Merkle branch proving transaction inclusion in a block.</summary>
    public ElectrumXTransactionMerkle(long blockHeight, IReadOnlyList<string> merkle, int position)
    {
        BlockHeight = blockHeight;
        Merkle = merkle;
        Position = position;
    }

    /// <summary>Height of the block containing the transaction.</summary>
    public long BlockHeight { get; }

    /// <summary>32-byte transaction hashes in the Merkle branch, each encoded as 64 hexadecimal characters.</summary>
    public IReadOnlyList<string> Merkle { get; }

    /// <summary>Zero-based transaction position in the block.</summary>
    public int Position { get; }
}
