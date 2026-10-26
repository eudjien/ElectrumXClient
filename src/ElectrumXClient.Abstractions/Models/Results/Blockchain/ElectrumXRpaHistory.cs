using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The history returned by blockchain.rpa.get_history.</summary>
public sealed class ElectrumXRpaHistory(IReadOnlyList<ElectrumXHistoryEntry> value)
    : ElectrumXResult
{
    /// <summary>History entries returned for the RPA prefix.</summary>
    public IReadOnlyList<ElectrumXHistoryEntry> Value { get; } = value;

    /// <summary>Converts the response to its history entries.</summary>
    /// <param name="response">The RPA history response.</param>
    /// <returns>The contained history entries.</returns>
    public static implicit operator ElectrumXHistoryEntry[](ElectrumXRpaHistory response)
    {
        return [.. response.Value];
    }
}
