namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.address.get_mempool.</summary>
public sealed class ElectrumXAddressMempoolQuery(string address)
    : ElectrumXQuery
{
    /// <summary>Gets the address whose mempool activity is requested.</summary>
    public string Address { get; } = ArgumentGuard.ValidateAddress(address, nameof(address));
}
