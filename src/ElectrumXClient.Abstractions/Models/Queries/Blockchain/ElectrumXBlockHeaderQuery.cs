namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.block.header without a checkpoint proof.</summary>
public sealed class ElectrumXBlockHeaderQuery : ElectrumXQuery
{
    /// <summary>Creates a query for a legacy block header.</summary>
    /// <param name="height">Zero-based block height.</param>
    public ElectrumXBlockHeaderQuery(long height)
    {
        ArgumentGuard.ThrowIfNegative(height, nameof(height));
        Height = height;
    }

    /// <summary>Gets the zero-based block height.</summary>
    public long Height { get; }
}
