namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A concatenated sequence of serialized block headers returned by blockchain.block.get_chunk.</summary>
public sealed class ElectrumXBlockHeaderChunk(string value) : ElectrumXResult
{
    /// <summary>Concatenated raw 80-byte block headers encoded as a hexadecimal string.</summary>
    public string Value { get; } = value;
}
