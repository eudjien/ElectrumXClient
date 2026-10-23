namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A script-hash status returned by the script-hash status RPC method.</summary>
public sealed class ElectrumXScriptHashStatus(string? status) : ElectrumXResult
{
    /// <summary>64-character hexadecimal history-status digest (SHA-256 when advertised by the server), or <see langword="null" /> when the script hash is unused.</summary>
    public string? Status { get; } = status;
}
