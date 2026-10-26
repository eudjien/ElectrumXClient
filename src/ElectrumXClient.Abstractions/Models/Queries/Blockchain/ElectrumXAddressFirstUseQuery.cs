namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.address.get_first_use.</summary>
public sealed class ElectrumXAddressFirstUseQuery(string address)
    : ElectrumXQuery
{
    /// <summary>Gets the address whose first use is requested.</summary>
    public string Address { get; } = ArgumentGuard.ValidateAddress(address, nameof(address));
}
