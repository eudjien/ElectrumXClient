namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The outcome of unsubscribing from transaction notifications.</summary>
public sealed class ElectrumXTransactionUnsubscription(bool wasUnsubscribed) : ElectrumXResult
{
    /// <summary>Whether the server removed the transaction subscription.</summary>
    public bool WasUnsubscribed { get; } = wasUnsubscribed;
}
