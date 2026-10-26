using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The history returned by blockchain.reusable.get_history.</summary>
public sealed class ElectrumXReusableHistory(IReadOnlyList<ElectrumXHistoryEntry> value)
    : ElectrumXResult
{
    /// <summary>History entries returned by the reusable-address method.</summary>
    public IReadOnlyList<ElectrumXHistoryEntry> Value { get; } = value;

    /// <summary>Converts the response to its history entries.</summary>
    /// <param name="response">The reusable-address history response.</param>
    /// <returns>The contained history entries.</returns>
    public static implicit operator ElectrumXHistoryEntry[](ElectrumXReusableHistory response)
    {
        return [.. response.Value];
    }
}
