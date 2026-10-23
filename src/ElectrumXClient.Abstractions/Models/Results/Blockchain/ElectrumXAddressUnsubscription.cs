namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The outcome of unsubscribing from address notifications.</summary>
public sealed class ElectrumXAddressUnsubscription(bool wasUnsubscribed) : ElectrumXResult
{
    /// <summary>Whether the server removed the address subscription.</summary>
    public bool WasUnsubscribed { get; } = wasUnsubscribed;
}
