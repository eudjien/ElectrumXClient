using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A transaction-history list returned by a blockchain RPC method.</summary>
public sealed class ElectrumXHistoryList(IReadOnlyList<ElectrumXHistoryEntry> value) : ElectrumXResult
{
    /// <summary>The returned transaction-history entries.</summary>
    public IReadOnlyList<ElectrumXHistoryEntry> Value { get; } = value;
}
