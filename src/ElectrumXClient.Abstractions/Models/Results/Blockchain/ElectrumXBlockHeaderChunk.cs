namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A concatenated sequence of serialized block headers returned by blockchain.block.get_chunk.</summary>
public sealed class ElectrumXBlockHeaderChunk(string value) : ElectrumXResult
{
    /// <summary>Concatenated raw 80-byte block headers encoded as a hexadecimal string.</summary>
    public string Value { get; } = value;

    /// <summary>Converts the response to its serialized headers.</summary>
    /// <param name="response">The block-header chunk response.</param>
    /// <returns>The chunk encoded as hexadecimal.</returns>
    public static implicit operator string(ElectrumXBlockHeaderChunk response)
    {
        return response.Value;
    }
}
