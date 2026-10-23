namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A block height returned by blockchain.numblocks.subscribe.</summary>
public sealed class ElectrumXBlockHeight(long height) : ElectrumXResult
{
    /// <summary>The current chain height.</summary>
    public long Height { get; } = height;
}
