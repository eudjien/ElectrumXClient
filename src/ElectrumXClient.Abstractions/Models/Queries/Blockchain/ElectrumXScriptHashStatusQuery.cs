namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.get_status.</summary>
public sealed class ElectrumXScriptHashStatusQuery(string scriptHash)
    : ElectrumXQuery
{
    /// <summary>Gets the script hash whose status is requested.</summary>
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));
}
