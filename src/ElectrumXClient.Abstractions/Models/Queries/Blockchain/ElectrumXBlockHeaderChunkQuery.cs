namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for the legacy blockchain.block.get_chunk method.</summary>
public sealed class ElectrumXBlockHeaderChunkQuery : ElectrumXQuery
{
    public ElectrumXBlockHeaderChunkQuery(long index)
    {
        ArgumentGuard.ThrowIfNegative(index, nameof(index));
        Index = index;
    }

    public long Index { get; }
}
