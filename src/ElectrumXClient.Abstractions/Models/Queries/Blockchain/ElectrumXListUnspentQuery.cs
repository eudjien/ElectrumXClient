namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.listunspent.</summary>
public sealed class ElectrumXListUnspentQuery(
    string scriptHash,
    string? tokenFilter = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXListUnspentQuery.TokenFilter) : ElectrumXQuery
{
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));

    public string? TokenFilter { get; } = tokenFilter;
}
