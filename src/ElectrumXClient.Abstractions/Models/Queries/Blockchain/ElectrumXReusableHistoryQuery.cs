namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for the legacy blockchain.reusable.get_history method.</summary>
public sealed class ElectrumXReusableHistoryQuery(long fromHeight, int count, string rpaPrefix) : ElectrumXQuery
{
    /// <summary>Gets the inclusive first block height to scan.</summary>
    public long FromHeight { get; } = fromHeight;

    /// <summary>Gets the number of blocks to scan.</summary>
    public int Count { get; } = count;

    /// <summary>Gets the hexadecimal reusable-payment-address prefix.</summary>
    public string RpaPrefix { get; } = ArgumentGuard.ValidateAndNormalizeRpaPrefix(rpaPrefix, nameof(rpaPrefix));
}
