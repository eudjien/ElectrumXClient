namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.get_first_use.</summary>
public sealed class ElectrumXScriptHashFirstUseQuery(string scriptHash)
    : ElectrumXQuery
{
    /// <summary>Gets the script hash whose first use is requested.</summary>
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));
}
