namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>The donation address returned by the server.</summary>
public sealed class ElectrumXDonationAddress : ElectrumXResult
{
    /// <summary>Creates a result containing the server donation address.</summary>
    public ElectrumXDonationAddress(string value)
    {
        Value = value;
    }

    /// <summary>Chain-specific address string returned by <c>server.donation_address</c> (for example, CashAddr or Base58).</summary>
    public string Value { get; }

    public static implicit operator string(ElectrumXDonationAddress response)
    {
        return response.Value;
    }
}
