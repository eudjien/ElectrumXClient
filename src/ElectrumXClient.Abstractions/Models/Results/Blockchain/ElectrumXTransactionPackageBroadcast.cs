using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The result returned by blockchain.transaction.broadcast_package.</summary>
public sealed class ElectrumXTransactionPackageBroadcast(
    bool success,
    IReadOnlyList<ElectrumXTransactionPackageError>? errors) : ElectrumXResult
{
    /// <summary>Whether the package was accepted.</summary>
    public bool Success { get; } = success;

    /// <summary>Package submission errors, when provided by the server.</summary>
    public IReadOnlyList<ElectrumXTransactionPackageError>? Errors { get; } = errors;
}

/// <summary>An error associated with a transaction rejected from a package submission.</summary>
public sealed class ElectrumXTransactionPackageError(string? transactionId, string? error)
{
    /// <summary>The transaction ID rejected by the server, when provided.</summary>
    public string? TransactionId { get; } = transactionId;

    /// <summary>The server's rejection reason, when provided.</summary>
    public string? Error { get; } = error;
}
