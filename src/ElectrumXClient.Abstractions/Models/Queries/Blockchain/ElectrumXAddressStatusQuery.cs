namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.address.get_status.</summary>
public sealed class ElectrumXAddressStatusQuery(string address)
    : ElectrumXQuery
{
    /// <summary>Gets the address whose status is requested.</summary>
    public string Address { get; } = ArgumentGuard.ValidateAddress(address, nameof(address));
}
