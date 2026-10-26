namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The estimated transaction fee per kilobyte returned by <c>blockchain.estimatefee</c>.</summary>
public sealed class ElectrumXEstimateFee : ElectrumXResult
{
    /// <summary>Creates an estimated fee result in whole coin units per kilobyte.</summary>
    public ElectrumXEstimateFee(decimal value)
    {
        Value = value;
    }

    /// <summary>The estimated fee in whole coin units per kilobyte, or <c>-1</c> if the server cannot estimate it.</summary>
    public decimal Value { get; }

    /// <summary>Converts the response to the estimated fee rate.</summary>
    /// <param name="response">The fee estimate response.</param>
    /// <returns>The fee rate in whole coin units per kilobyte.</returns>
    public static implicit operator decimal(ElectrumXEstimateFee response)
    {
        return response.Value;
    }
}
