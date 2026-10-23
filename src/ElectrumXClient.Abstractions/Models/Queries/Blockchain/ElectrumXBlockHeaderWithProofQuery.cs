using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.block.header with a checkpoint proof.</summary>
public sealed class ElectrumXBlockHeaderWithProofQuery : ElectrumXQuery
{
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

    public long Height { get; }

    public long CheckpointHeight { get; }
}
