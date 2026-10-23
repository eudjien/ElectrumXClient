namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a blockchain.scripthash.subscribe notification.</summary>
public sealed class ElectrumXScriptHashNotification(string scriptHash, string? status) : ElectrumXNotification
{
    public string ScriptHash { get; } = scriptHash;

    public string? Status { get; } = status;
}
