using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.block.header with a checkpoint proof.</summary>
public sealed class ElectrumXBlockHeaderWithProofQuery : ElectrumXQuery
{
    /// <summary>Creates a block-header query that includes an inclusion proof at a checkpoint.</summary>
    /// <param name="height">Zero-based height of the requested header.</param>
    /// <param name="checkpointHeight">Height of the checkpoint used to construct the proof.</param>
    public ElectrumXBlockHeaderWithProofQuery(long height, long checkpointHeight)
    {
        ArgumentGuard.ThrowIfNegative(height, nameof(height));
        if (checkpointHeight < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(checkpointHeight), "Checkpoint height must be greater than zero.");
        }

        if (height > checkpointHeight)
        {
            throw new ArgumentOutOfRangeException(nameof(checkpointHeight), "Checkpoint height must not be below the requested height.");
        }

        Height = height;
        CheckpointHeight = checkpointHeight;
    }

    /// <summary>Gets the requested header height.</summary>
    public long Height { get; }

    /// <summary>Gets the checkpoint height used for the inclusion proof.</summary>
    public long CheckpointHeight { get; }
}
