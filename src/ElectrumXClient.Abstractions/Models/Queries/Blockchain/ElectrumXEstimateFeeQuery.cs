namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.estimatefee.</summary>
public sealed class ElectrumXEstimateFeeQuery : ElectrumXQuery
{
    /// <summary>Creates an estimate-fee query.</summary>
    public ElectrumXEstimateFeeQuery(
        int numberOfBlocks,
        string? mode = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXEstimateFeeQuery.Mode)
    {
        NumberOfBlocks = numberOfBlocks;
        Mode = mode;
    }

    public int NumberOfBlocks { get; }

    public string? Mode { get; }
}
