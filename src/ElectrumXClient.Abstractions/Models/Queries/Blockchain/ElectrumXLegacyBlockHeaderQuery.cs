namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for the legacy blockchain.block.get_header method.</summary>
public sealed class ElectrumXLegacyBlockHeaderQuery : ElectrumXQuery
{
    /// <summary>Creates a query for a legacy block header at a height.</summary>
    /// <param name="height">Zero-based block height.</param>
    public ElectrumXLegacyBlockHeaderQuery(long height)
    {
        ArgumentGuard.ThrowIfNegative(height, nameof(height));
        Height = height;
    }

    /// <summary>Gets the zero-based block height.</summary>
    public long Height { get; }
}
