using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A mempool-entry list returned by a blockchain RPC method.</summary>
public sealed class ElectrumXMempoolEntryList(IReadOnlyList<ElectrumXMempoolEntry> value) : ElectrumXResult
{
    /// <summary>The returned mempool entries.</summary>
    public IReadOnlyList<ElectrumXMempoolEntry> Value { get; } = value;
}
