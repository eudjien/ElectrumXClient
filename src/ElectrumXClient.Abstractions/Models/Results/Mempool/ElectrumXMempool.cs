using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;

/// <summary>The mempool transactions returned for a script hash.</summary>
public sealed class ElectrumXMempool : ElectrumXResult
{
    /// <summary>Creates a result containing unconfirmed transactions for a script hash.</summary>
    public ElectrumXMempool(IReadOnlyList<ElectrumXMempoolEntry> value)
    {
        Value = value;
    }

    /// <summary>Mempool entries in server-defined arbitrary order.</summary>
    public IReadOnlyList<ElectrumXMempoolEntry> Value { get; }

    public static implicit operator ElectrumXMempoolEntry[](ElectrumXMempool response)
    {
        return [.. response.Value];
    }
}
