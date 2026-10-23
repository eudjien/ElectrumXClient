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

    public static implicit operator string(ElectrumXRawTransaction response)
    {
        return response.Value;
    }
}
