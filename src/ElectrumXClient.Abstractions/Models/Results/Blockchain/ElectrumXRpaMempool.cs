using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The mempool entries returned by blockchain.rpa.get_mempool.</summary>
public sealed class ElectrumXRpaMempool(IReadOnlyList<ElectrumXMempoolEntry> value)
    : ElectrumXResult
{
    /// <summary>Mempool entries matching the RPA prefix.</summary>
    public IReadOnlyList<ElectrumXMempoolEntry> Value { get; } = value;

    /// <summary>Converts the response to its mempool entries.</summary>
    /// <param name="response">The RPA mempool response.</param>
    /// <returns>The contained mempool entries.</returns>
    public static implicit operator ElectrumXMempoolEntry[](ElectrumXRpaMempool response)
    {
        return [.. response.Value];
    }
}
