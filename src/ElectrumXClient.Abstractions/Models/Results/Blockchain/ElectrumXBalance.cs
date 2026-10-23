namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Confirmed and unconfirmed balances returned for a script hash.</summary>
public sealed class ElectrumXBalance : ElectrumXResult
{
    /// <summary>Creates a balance result in the coin's smallest units.</summary>
    public ElectrumXBalance(long confirmed, long unconfirmed)
    {
        Confirmed = confirmed;
        Unconfirmed = unconfirmed;
    }

    /// <summary>Confirmed balance in the coin's smallest units (satoshis on Bitcoin).</summary>
    public long Confirmed { get; }

    /// <summary>Unconfirmed balance in the coin's smallest units (satoshis on Bitcoin).</summary>
    public long Unconfirmed { get; }
}
