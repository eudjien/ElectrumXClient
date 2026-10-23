namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>An address status returned by the address subscription RPC method.</summary>
public sealed class ElectrumXAddressSubscription(string? status) : ElectrumXResult
{
    /// <summary>64-character hexadecimal history-status digest (SHA-256 when advertised by the server), or <see langword="null" /> when the address is unused.</summary>
    public string? Status { get; } = status;
}
