namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a blockchain.address.subscribe notification.</summary>
public sealed class ElectrumXAddressNotification(string address, string? status) : ElectrumXNotification
{
    /// <summary>The address whose status changed.</summary>
    public string Address { get; } = address;

    /// <summary>The address status, or <see langword="null" /> when it has no history.</summary>
    public string? Status { get; } = status;
}
