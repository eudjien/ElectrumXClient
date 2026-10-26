namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for the legacy blockchain.block.get_chunk method.</summary>
public sealed class ElectrumXBlockHeaderChunkQuery : ElectrumXQuery
{
    /// <summary>Creates a query for a legacy block-header chunk.</summary>
    /// <param name="index">Zero-based chunk index.</param>
    public ElectrumXBlockHeaderChunkQuery(long index)
    {
        ArgumentGuard.ThrowIfNegative(index, nameof(index));
        Index = index;
    }

    /// <summary>Gets the zero-based chunk index.</summary>
    public long Index { get; }
}
