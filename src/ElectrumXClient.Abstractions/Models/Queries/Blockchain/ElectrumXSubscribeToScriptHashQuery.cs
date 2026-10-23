namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.subscribe.</summary>
public sealed class ElectrumXSubscribeToScriptHashQuery(string scriptHash) : ElectrumXQuery
{
    public string ScriptHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));
}
