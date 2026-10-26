namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The serialized block header returned by the legacy blockchain.block.get_header method.</summary>
public sealed class ElectrumXLegacyBlockHeader(string value) : ElectrumXResult
{
    /// <summary>Raw 80-byte block header encoded as 160 hexadecimal characters.</summary>
    public string Value { get; } = value;

    /// <summary>Converts the response to the serialized legacy header.</summary>
    /// <param name="response">The legacy block-header response.</param>
    /// <returns>The header encoded as hexadecimal.</returns>
    public static implicit operator string(ElectrumXLegacyBlockHeader response)
    {
        return response.Value;
    }
}
