namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.address.get_balance.</summary>
public sealed class ElectrumXAddressBalanceQuery(
    string address,
    ElectrumXTokenFilter? tokenFilter = null) : ElectrumXQuery
{
    /// <summary>Gets the address whose balance is requested.</summary>
    public string Address { get; } = ArgumentGuard.ValidateAddress(address, nameof(address));

    /// <summary>Gets the optional filter that controls which token outputs are included.</summary>
    public ElectrumXTokenFilter? TokenFilter { get; } = ArgumentGuard.ValidateTokenFilter(tokenFilter, nameof(tokenFilter));
}
