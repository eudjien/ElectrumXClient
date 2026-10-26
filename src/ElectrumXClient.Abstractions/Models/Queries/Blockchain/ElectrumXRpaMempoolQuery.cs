namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.rpa.get_mempool.</summary>
public sealed class ElectrumXRpaMempoolQuery(string rpaPrefix) : ElectrumXQuery
{
    /// <summary>Gets the hexadecimal reusable-payment-address prefix.</summary>
    public string RpaPrefix { get; } = ArgumentGuard.ValidateAndNormalizeRpaPrefix(rpaPrefix, nameof(rpaPrefix));
}
