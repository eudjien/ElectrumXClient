namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.get_balance.</summary>
public sealed class ElectrumXBalanceQuery(
    string scriptHash,
    string? tokenFilter = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBalanceQuery.TokenFilter) : ElectrumXQuery
{
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));

    public string? TokenFilter { get; } = tokenFilter;
}
