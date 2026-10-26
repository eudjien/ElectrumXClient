namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.listunspent.</summary>
public sealed class ElectrumXListUnspentQuery(
    string scriptHash,
    ElectrumXTokenFilter? tokenFilter = null) : ElectrumXQuery
{
    /// <summary>Gets the script hash whose unspent outputs are requested.</summary>
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));

    /// <summary>Gets the optional filter that controls which token outputs are included.</summary>
    public ElectrumXTokenFilter? TokenFilter { get; } =
        ArgumentGuard.ValidateTokenFilter(tokenFilter, nameof(tokenFilter));
}
