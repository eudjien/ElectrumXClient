using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The transaction history returned by blockchain.scripthash.get_history.</summary>
public sealed class ElectrumXHistory : ElectrumXResult
{
    /// <summary>Creates a result containing confirmed history followed by mempool entries.</summary>
    public ElectrumXHistory(IReadOnlyList<ElectrumXHistoryEntry> value)
    {
        Value = value;
    }

    /// <summary>History entries in blockchain order, followed by the server's mempool entries.</summary>
    public IReadOnlyList<ElectrumXHistoryEntry> Value { get; }

    public static implicit operator ElectrumXHistoryEntry[](ElectrumXHistory response)
    {
        return [.. response.Value];
    }
}
