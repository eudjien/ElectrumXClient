using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.broadcast_package.</summary>
public sealed class ElectrumXBroadcastPackageQuery : ElectrumXQuery
{
    /// <summary>Creates a transaction-package broadcast query.</summary>
    public ElectrumXBroadcastPackageQuery(
        IReadOnlyList<string> rawTransactions,
        bool verbose = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBroadcastPackageQuery.Verbose)
    {
        ArgumentGuard.ThrowIfNull(rawTransactions, nameof(rawTransactions));
        RawTransactions = rawTransactions;
        Verbose = verbose;
    }

    /// <summary>Raw transactions in topological package order.</summary>
    public IReadOnlyList<string> RawTransactions { get; }

    /// <summary>Whether the daemon-specific verbose result is requested.</summary>
    public bool Verbose { get; }
}
