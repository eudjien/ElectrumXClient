namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A script hash returned by the address script-hash RPC method.</summary>
public sealed class ElectrumXAddressScriptHash(string scriptHash) : ElectrumXResult
{
    /// <summary>Script hash: 32-byte digest of the locking script (SHA-256 when advertised by the server), reversed and encoded as 64 hexadecimal characters.</summary>
    public string ScriptHash { get; } = scriptHash;
}
