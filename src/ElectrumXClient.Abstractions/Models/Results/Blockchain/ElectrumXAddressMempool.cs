using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The mempool entries returned by blockchain.address.get_mempool.</summary>
public sealed class ElectrumXAddressMempool(IReadOnlyList<ElectrumXMempoolEntry> value)
    : ElectrumXResult
{
    /// <summary>Mempool entries returned for the address.</summary>
    public IReadOnlyList<ElectrumXMempoolEntry> Value { get; } = value;

    /// <summary>Converts the response to its mempool entries.</summary>
    /// <param name="response">The address mempool response.</param>
    /// <returns>The contained mempool entries.</returns>
    public static implicit operator ElectrumXMempoolEntry[](ElectrumXAddressMempool response)
    {
        return [.. response.Value];
    }
}
