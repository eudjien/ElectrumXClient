namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for the legacy blockchain.block.get_header method.</summary>
public sealed class ElectrumXLegacyBlockHeaderQuery : ElectrumXQuery
{
    public ElectrumXLegacyBlockHeaderQuery(long height)
    {
        ArgumentGuard.ThrowIfNegative(height, nameof(height));
        Height = height;
    }

    public long Height { get; }
}
