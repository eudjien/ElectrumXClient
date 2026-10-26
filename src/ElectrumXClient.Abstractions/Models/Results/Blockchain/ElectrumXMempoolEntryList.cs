using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A mempool-entry list returned by a blockchain RPC method.</summary>
public sealed class ElectrumXMempoolEntryList(IReadOnlyList<ElectrumXMempoolEntry> value) : ElectrumXResult
{
    /// <summary>The returned mempool entries.</summary>
    public IReadOnlyList<ElectrumXMempoolEntry> Value { get; } = value;

    /// <summary>Converts the response to its mempool entries.</summary>
    /// <param name="response">The mempool-entry list response.</param>
    /// <returns>The contained mempool entries.</returns>
    public static implicit operator ElectrumXMempoolEntry[](ElectrumXMempoolEntryList response)
    {
        return [.. response.Value];
    }
}
