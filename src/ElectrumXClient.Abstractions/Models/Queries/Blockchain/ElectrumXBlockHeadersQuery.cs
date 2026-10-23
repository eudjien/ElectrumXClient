using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.block.headers.</summary>
public sealed class ElectrumXBlockHeadersQuery : ElectrumXQuery
{
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

    public long StartHeight { get; }

    public int Count { get; }

    public long CheckpointHeight { get; }
}
