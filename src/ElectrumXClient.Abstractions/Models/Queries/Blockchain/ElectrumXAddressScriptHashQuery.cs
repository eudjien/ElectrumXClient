namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.address.get_scripthash.</summary>
public sealed class ElectrumXAddressScriptHashQuery(string address)
    : ElectrumXQuery
{
    /// <summary>Gets the address to convert to a script hash.</summary>
    public string Address { get; } = ArgumentGuard.ValidateAddress(address, nameof(address));
}
