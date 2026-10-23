namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The outcome of unsubscribing from block-header notifications.</summary>
public sealed class ElectrumXHeadersUnsubscription(bool wasUnsubscribed) : ElectrumXResult
{
    /// <summary>Whether the server removed the block-header subscription.</summary>
    public bool WasUnsubscribed { get; } = wasUnsubscribed;
}
