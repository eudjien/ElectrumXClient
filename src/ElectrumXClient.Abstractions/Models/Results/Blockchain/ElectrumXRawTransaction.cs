namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The serialized transaction returned by blockchain.transaction.get.</summary>
public sealed class ElectrumXRawTransaction : ElectrumXResult
{
    /// <summary>Creates a result containing a serialized transaction.</summary>
    public ElectrumXRawTransaction(string value)
    {
        Value = value;
    }

    /// <summary>Serialized transaction bytes encoded as hexadecimal.</summary>
    public string Value { get; }

    /// <summary>Converts the response to the raw transaction.</summary>
    /// <param name="response">The raw transaction response.</param>
    /// <returns>The transaction serialized as hexadecimal.</returns>
    public static implicit operator string(ElectrumXRawTransaction response)
    {
        return response.Value;
    }
}
