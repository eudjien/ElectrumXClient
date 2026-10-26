using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The mempool entries returned by blockchain.reusable.get_mempool.</summary>
public sealed class ElectrumXReusableMempool(IReadOnlyList<ElectrumXMempoolEntry> value)
    : ElectrumXResult
{
    /// <summary>Mempool entries returned by the reusable-address method.</summary>
    public IReadOnlyList<ElectrumXMempoolEntry> Value { get; } = value;

    /// <summary>Converts the response to its mempool entries.</summary>
    /// <param name="response">The reusable-address mempool response.</param>
    /// <returns>The contained mempool entries.</returns>
    public static implicit operator ElectrumXMempoolEntry[](ElectrumXReusableMempool response)
    {
        return [.. response.Value];
    }
}
