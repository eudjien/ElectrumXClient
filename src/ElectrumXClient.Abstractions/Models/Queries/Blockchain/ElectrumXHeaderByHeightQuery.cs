namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for retrieving a block header by height.</summary>
public sealed class ElectrumXHeaderByHeightQuery(long height)
    : ElectrumXQuery
{
    /// <summary>Gets the requested block height.</summary>
    public long Height { get; } = height;
}
