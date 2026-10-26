namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The balance returned by blockchain.address.get_balance.</summary>
public sealed class ElectrumXAddressBalance(long confirmed, long unconfirmed) : ElectrumXResult
{
    /// <summary>Confirmed balance in the coin's smallest units.</summary>
    public long Confirmed { get; } = confirmed;

    /// <summary>Unconfirmed balance in the coin's smallest units.</summary>
    public long Unconfirmed { get; } = unconfirmed;
}
