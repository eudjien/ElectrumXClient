namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The serialized block header returned by blockchain.block.header.</summary>
public sealed class ElectrumXBlockHeader : ElectrumXResult
{
    /// <summary>Creates a result containing a serialized block header.</summary>
    public ElectrumXBlockHeader(string value)
    {
        Value = value;
    }

    /// <summary>Raw 80-byte block header encoded as 160 hexadecimal characters.</summary>
    public string Value { get; }

    /// <summary>Converts the response to the serialized header.</summary>
    /// <param name="response">The block-header response.</param>
    /// <returns>The header encoded as hexadecimal.</returns>
    public static implicit operator string(ElectrumXBlockHeader response)
    {
        return response.Value;
    }
}
