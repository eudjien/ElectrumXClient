namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The minimum relay fee rate returned by blockchain.relayfee.</summary>
public sealed class ElectrumXRelayFee : ElectrumXResult
{
    /// <summary>Creates a result containing the server's minimum relay fee rate.</summary>
    public ElectrumXRelayFee(decimal value)
    {
        Value = value;
    }

    /// <summary>Minimum relay fee in whole coin units per kilobyte.</summary>
    public decimal Value { get; }

    public static implicit operator decimal(ElectrumXRelayFee response)
    {
        return response.Value;
    }
}
