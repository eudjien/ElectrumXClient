using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.ProTx;

/// <summary>Query parameters for protx.diff.</summary>
public sealed class ElectrumXProTxDiffQuery : ElectrumXQuery
{
    /// <summary>Creates a query for the ProTx state changes between two block heights.</summary>
    /// <param name="baseHeight">Starting height of the diff; must be at least one.</param>
    /// <param name="height">Ending height, which must not precede <paramref name="baseHeight" />.</param>
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

    /// <summary>Gets the starting height of the diff.</summary>
    public long BaseHeight { get; }

    /// <summary>Gets the ending height of the diff.</summary>
    public long Height { get; }
}
