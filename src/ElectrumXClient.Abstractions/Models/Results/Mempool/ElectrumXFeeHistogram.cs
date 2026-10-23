using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;

/// <summary>The fee-rate buckets returned by mempool.get_fee_histogram.</summary>
public sealed class ElectrumXFeeHistogram : ElectrumXResult
{
    /// <summary>Creates a result containing the server's fee histogram buckets.</summary>
    public ElectrumXFeeHistogram(IReadOnlyList<ElectrumXFeeHistogramEntry> value)
    {
        Value = value;
    }

    /// <summary>Fee-rate buckets in server-provided order; each bucket reports cumulative virtual size.</summary>
    public IReadOnlyList<ElectrumXFeeHistogramEntry> Value { get; }

    public static implicit operator ElectrumXFeeHistogramEntry[](ElectrumXFeeHistogram response)
    {
        return [.. response.Value];
    }
}
