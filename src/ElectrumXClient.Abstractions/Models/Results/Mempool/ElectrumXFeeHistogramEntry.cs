namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;

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
