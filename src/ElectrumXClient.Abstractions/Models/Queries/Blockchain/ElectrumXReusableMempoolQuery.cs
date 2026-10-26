namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for the legacy blockchain.reusable.get_mempool method.</summary>
public sealed class ElectrumXReusableMempoolQuery(string rpaPrefix) : ElectrumXQuery
{
    /// <summary>Gets the hexadecimal reusable-payment-address prefix.</summary>
    public string RpaPrefix { get; } = ArgumentGuard.ValidateAndNormalizeRpaPrefix(rpaPrefix, nameof(rpaPrefix));
}
