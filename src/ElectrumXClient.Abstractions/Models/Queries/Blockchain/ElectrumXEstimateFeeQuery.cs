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

    /// <summary>Gets the target number of blocks for transaction confirmation.</summary>
    public int NumberOfBlocks { get; }

    /// <summary>Gets the optional fee-estimation mode supported by the server.</summary>
    public string? Mode { get; }
}
