using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The transaction history returned by blockchain.address.get_history.</summary>
public sealed class ElectrumXAddressHistory(IReadOnlyList<ElectrumXHistoryEntry> value)
    : ElectrumXResult
{
    /// <summary>History entries returned for the address.</summary>
    public IReadOnlyList<ElectrumXHistoryEntry> Value { get; } = value;

    /// <summary>Converts the response to its history entries.</summary>
    /// <param name="response">The address history response.</param>
    /// <returns>The contained history entries.</returns>
    public static implicit operator ElectrumXHistoryEntry[](ElectrumXAddressHistory response)
    {
        return [.. response.Value];
    }
}
