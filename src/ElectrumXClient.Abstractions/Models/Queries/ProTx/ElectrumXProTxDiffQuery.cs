using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.ProTx;

/// <summary>Query parameters for protx.diff.</summary>
public sealed class ElectrumXProTxDiffQuery : ElectrumXQuery
{
    public ElectrumXProTxDiffQuery(long baseHeight, long height)
    {
        ArgumentGuard.ThrowIfNegative(baseHeight, nameof(baseHeight));
        ArgumentGuard.ThrowIfNegative(height, nameof(height));

        if (baseHeight < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(baseHeight), "Base height must be at least 1.");
        }

        if (height < baseHeight)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than or equal to base height.");
        }

        BaseHeight = baseHeight;
        Height = height;
    }

    public long BaseHeight { get; }

    public long Height { get; }
}
