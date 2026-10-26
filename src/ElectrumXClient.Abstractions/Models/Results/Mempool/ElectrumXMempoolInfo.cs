using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;

/// <summary>Optional mempool policy details exposed by the server's backing daemon.</summary>
public sealed class ElectrumXMempoolInfo(
    decimal? mempoolMinimumFee,
    decimal? minimumRelayFee,
    decimal? incrementalRelayFee,
    int? unbroadcastCount,
    bool? fullReplaceByFee,
    IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null) : ElectrumXResult
{
    /// <summary>Minimum fee rate required for mempool acceptance.</summary>
    public decimal? MempoolMinimumFee { get; } = mempoolMinimumFee;

    /// <summary>Minimum relay fee rate.</summary>
    public decimal? MinimumRelayFee { get; } = minimumRelayFee;

    /// <summary>Incremental relay fee rate.</summary>
    public decimal? IncrementalRelayFee { get; } = incrementalRelayFee;

    /// <summary>Number of unbroadcast transactions.</summary>
    public int? UnbroadcastCount { get; } = unbroadcastCount;

    /// <summary>Whether full replace-by-fee is enabled.</summary>
    public bool? FullReplaceByFee { get; } = fullReplaceByFee;

    /// <summary>Additional daemon-specific fields returned by mempool.get_info.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; } =
        CopyAdditionalProperties(additionalProperties);

    private static IReadOnlyDictionary<string, ElectrumXJsonValue> CopyAdditionalProperties(
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties)
    {
        var copy = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);

        if (additionalProperties is not null)
        {
            foreach ((string name, var value) in additionalProperties)
            {
                copy[name] = value;
            }
        }

        return new ReadOnlyDictionary<string, ElectrumXJsonValue>(copy);
    }
}
