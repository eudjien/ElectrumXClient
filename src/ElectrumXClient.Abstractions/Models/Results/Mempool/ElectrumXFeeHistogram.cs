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

    /// <summary>Converts the response to its fee histogram entries.</summary>
    /// <param name="response">The fee histogram response.</param>
    /// <returns>The contained histogram entries.</returns>
    public static implicit operator ElectrumXFeeHistogramEntry[](ElectrumXFeeHistogram response)
    {
        return [.. response.Value];
    }
}

/// <summary>A fee-rate bucket in the mempool fee histogram.</summary>
public sealed class ElectrumXFeeHistogramEntry : ElectrumXResult
{
    /// <summary>Creates a fee histogram bucket.</summary>
    public ElectrumXFeeHistogramEntry(decimal feeRate, long virtualSize)
    {
        FeeRate = feeRate;
        VirtualSize = virtualSize;
    }

    /// <summary>Fee rate associated with the histogram bucket.</summary>
    public decimal FeeRate { get; }

    /// <summary>Cumulative virtual size of transactions in the bucket, in virtual bytes.</summary>
    public long VirtualSize { get; }
}
