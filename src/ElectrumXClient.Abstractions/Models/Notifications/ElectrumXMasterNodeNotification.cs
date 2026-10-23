namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a masternode.subscribe notification.</summary>
public sealed class ElectrumXMasterNodeNotification(string collateral, string? status) : ElectrumXNotification
{
    public string Collateral { get; } = collateral;

    public string? Status { get; } = status;
}
