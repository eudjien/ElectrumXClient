namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a blockchain.numblocks.subscribe notification.</summary>
public sealed class ElectrumXBlockHeightNotification(long height) : ElectrumXNotification
{
    /// <summary>The current chain height.</summary>
    public long Height { get; } = height;
}
