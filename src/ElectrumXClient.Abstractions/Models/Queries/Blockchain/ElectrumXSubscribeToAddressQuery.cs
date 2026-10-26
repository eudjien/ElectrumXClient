namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for subscribing to address status updates.</summary>
public sealed class ElectrumXSubscribeToAddressQuery(string address)
    : ElectrumXQuery
{
    /// <summary>Gets the address to monitor.</summary>
    public string Address { get; } = ArgumentGuard.ValidateAddress(address, nameof(address));
}
