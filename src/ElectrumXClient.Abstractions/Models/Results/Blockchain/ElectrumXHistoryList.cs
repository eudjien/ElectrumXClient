using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A transaction-history list returned by a blockchain RPC method.</summary>
public sealed class ElectrumXHistoryList(IReadOnlyList<ElectrumXHistoryEntry> value) : ElectrumXResult
{
    /// <summary>The returned transaction-history entries.</summary>
    public IReadOnlyList<ElectrumXHistoryEntry> Value { get; } = value;

    /// <summary>Converts the response to its history entries.</summary>
    /// <param name="response">The history-list response.</param>
    /// <returns>The contained history entries.</returns>
    public static implicit operator ElectrumXHistoryEntry[](ElectrumXHistoryList response)
    {
        return [.. response.Value];
    }
}
