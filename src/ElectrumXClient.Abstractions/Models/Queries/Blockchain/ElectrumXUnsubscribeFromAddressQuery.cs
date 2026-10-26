namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for unsubscribing from address status updates.</summary>
public sealed class ElectrumXUnsubscribeFromAddressQuery(string address)
    : ElectrumXQuery
{
    /// <summary>Gets the address to stop monitoring.</summary>
    public string Address { get; } = ArgumentGuard.ValidateAddress(address, nameof(address));
}
