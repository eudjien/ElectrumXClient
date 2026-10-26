namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a masternode.subscribe notification.</summary>
public sealed class ElectrumXMasterNodeNotification(string collateral, string? status) : ElectrumXNotification
{
    /// <summary>Gets the masternode collateral identifier.</summary>
    public string Collateral { get; } = collateral;

    /// <summary>Gets the updated masternode status, when supplied by the server.</summary>
    public string? Status { get; } = status;
}
