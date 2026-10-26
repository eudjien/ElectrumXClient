namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a blockchain.scripthash.subscribe notification.</summary>
public sealed class ElectrumXScriptHashNotification(string scriptHash, string? status) : ElectrumXNotification
{
    /// <summary>Gets the script hash associated with the notification.</summary>
    public string ScriptHash { get; } = scriptHash;

    /// <summary>Gets the updated status string, or <see langword="null" /> when the script hash is unused.</summary>
    public string? Status { get; } = status;
}
