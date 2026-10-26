namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.get_mempool.</summary>
public sealed class ElectrumXMempoolQuery(string scriptHash) : ElectrumXQuery
{
    /// <summary>Gets the script hash whose mempool activity is requested.</summary>
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));
}
