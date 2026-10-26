namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.get_balance.</summary>
public sealed class ElectrumXBalanceQuery(
    string scriptHash,
    ElectrumXTokenFilter? tokenFilter = null) : ElectrumXQuery
{
    /// <summary>Gets the script hash whose balance is requested.</summary>
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));

    /// <summary>Gets the optional filter that controls which token outputs are included.</summary>
    public ElectrumXTokenFilter? TokenFilter { get; } =
        ArgumentGuard.ValidateTokenFilter(tokenFilter, nameof(tokenFilter));
}
