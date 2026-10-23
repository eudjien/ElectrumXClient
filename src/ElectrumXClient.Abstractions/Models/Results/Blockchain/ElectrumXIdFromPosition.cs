using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Transaction hash and optional Merkle branch for a transaction at a block position.</summary>
public sealed class ElectrumXIdFromPosition : ElectrumXResult
{
    /// <summary>Creates a transaction ID result with an optional Merkle proof.</summary>
    public ElectrumXIdFromPosition(string transactionHash, IReadOnlyList<string>? merkle = null)
    {
        TransactionHash = transactionHash;
        Merkle = merkle;
    }

    /// <summary>32-byte transaction hash at the requested block position, encoded as 64 hexadecimal characters.</summary>
    public string TransactionHash { get; }

    /// <summary>32-byte transaction hashes in the Merkle branch, each encoded as 64 hexadecimal characters, or <see langword="null" /> when not requested.</summary>
    public IReadOnlyList<string>? Merkle { get; }
}
