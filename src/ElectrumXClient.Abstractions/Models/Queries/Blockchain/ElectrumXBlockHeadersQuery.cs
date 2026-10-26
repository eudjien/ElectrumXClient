using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.block.headers.</summary>
public sealed class ElectrumXBlockHeadersQuery : ElectrumXQuery
{
    /// <summary>Creates a query for a range of block headers, optionally with a checkpoint proof.</summary>
    /// <param name="startHeight">Zero-based height of the first header.</param>
    /// <param name="count">Number of headers requested.</param>
    /// <param name="checkpointHeight">Checkpoint height, or zero to omit a proof.</param>
    public ElectrumXBlockHeadersQuery(
        long startHeight,
        int count,
        long checkpointHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBlockHeadersQuery.CheckpointHeight)
    {
        ArgumentGuard.ThrowIfNegative(startHeight, nameof(startHeight));
        ArgumentGuard.ThrowIfNegative(count, nameof(count));
        ArgumentGuard.ThrowIfNegative(checkpointHeight, nameof(checkpointHeight));

        if (checkpointHeight > 0 &&
            (count > 0 && startHeight > checkpointHeight - (count - 1L) ||
             count == 0 && startHeight > checkpointHeight && startHeight - checkpointHeight > 1))
        {
            throw new ArgumentOutOfRangeException(nameof(checkpointHeight), "The requested header range must not exceed the checkpoint height.");
        }

        StartHeight = startHeight;
        Count = count;
        CheckpointHeight = checkpointHeight;
    }

    /// <summary>Gets the zero-based height of the first header.</summary>
    public long StartHeight { get; }

    /// <summary>Gets the number of headers requested.</summary>
    public int Count { get; }

    /// <summary>Gets the checkpoint height, or zero when no proof is requested.</summary>
    public long CheckpointHeight { get; }
}
