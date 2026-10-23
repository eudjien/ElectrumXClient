namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.block.header without a checkpoint proof.</summary>
public sealed class ElectrumXBlockHeaderQuery : ElectrumXQuery
{
    public ElectrumXBlockHeaderQuery(long height)
    {
        ArgumentGuard.ThrowIfNegative(height, nameof(height));
        Height = height;
    }

    public long Height { get; }
}
