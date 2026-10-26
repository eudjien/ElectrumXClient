using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.rpa.get_history.</summary>
public sealed class ElectrumXRpaHistoryQuery : ElectrumXQuery
{
    /// <summary>Creates an RPA history query for a prefix and an optional height range.</summary>
    /// <param name="rpaPrefix">Hexadecimal reusable-payment-address prefix.</param>
    /// <param name="fromHeight">Inclusive first block height.</param>
    /// <param name="toHeight">Exclusive end height, or <c>-1</c> to scan to the server's limit.</param>
    public ElectrumXRpaHistoryQuery(
        string rpaPrefix,
        long fromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXRpaHistoryQuery.ToHeight)
    {
        if (fromHeight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fromHeight));
        }

        if (toHeight < ElectrumXJsonMethodArgumentDefaultValues.ElectrumXRpaHistoryQuery.ToHeight ||
            toHeight != ElectrumXJsonMethodArgumentDefaultValues.ElectrumXRpaHistoryQuery.ToHeight && fromHeight > toHeight)
        {
            throw new ArgumentOutOfRangeException(
                nameof(toHeight),
                toHeight,
                "To height must be -1 or non-negative and must be greater than or equal to from height.");
        }

        RpaPrefix = ArgumentGuard.ValidateAndNormalizeRpaPrefix(rpaPrefix, nameof(rpaPrefix));
        FromHeight = fromHeight;
        ToHeight = toHeight;
    }

    /// <summary>Gets the hexadecimal reusable-payment-address prefix.</summary>
    public string RpaPrefix { get; }

    /// <summary>Gets the inclusive first block height to scan.</summary>
    public long FromHeight { get; }

    /// <summary>Gets the exclusive end height, or <c>-1</c> for the server-defined limit.</summary>
    public long ToHeight { get; }
}
