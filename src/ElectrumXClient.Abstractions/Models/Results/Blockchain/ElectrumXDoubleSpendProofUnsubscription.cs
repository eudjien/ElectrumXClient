namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The outcome of unsubscribing from double-spend-proof notifications.</summary>
public sealed class ElectrumXDoubleSpendProofUnsubscription(bool wasUnsubscribed) : ElectrumXResult
{
    /// <summary>Whether the server removed the double-spend-proof subscription.</summary>
    public bool WasUnsubscribed { get; } = wasUnsubscribed;
}
