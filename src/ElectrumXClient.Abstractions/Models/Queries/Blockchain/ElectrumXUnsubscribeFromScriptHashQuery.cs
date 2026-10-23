namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.unsubscribe.</summary>
public sealed class ElectrumXUnsubscribeFromScriptHashQuery(string scriptHash) : ElectrumXQuery
{
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));
}
