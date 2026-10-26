using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.address.get_history.</summary>
public sealed class ElectrumXAddressHistoryQuery : ElectrumXQuery
{
    /// <summary>Creates a history query for an address and an optional height range.</summary>
    /// <param name="address">The address whose history is requested.</param>
    /// <param name="fromHeight">Inclusive first block height; defaults to zero.</param>
    /// <param name="toHeight">Exclusive end height, or <c>-1</c> to scan to the server's limit.</param>
    public ElectrumXAddressHistoryQuery(
        string address,
        long fromHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.FromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.ToHeight)
    {
        if (fromHeight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fromHeight));
        }

        if (toHeight < ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.ToHeight ||
            toHeight != ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.ToHeight && fromHeight > toHeight)
        {
            throw new ArgumentOutOfRangeException(nameof(toHeight));
        }

        Address = ArgumentGuard.ValidateAddress(address, nameof(address));
        FromHeight = fromHeight;
        ToHeight = toHeight;
    }

    /// <summary>Gets the address whose history is requested.</summary>
    public string Address { get; }

    /// <summary>Gets the inclusive first block height to scan.</summary>
    public long FromHeight { get; }

    /// <summary>Gets the exclusive end height, or <c>-1</c> for the server-defined limit.</summary>
    public long ToHeight { get; }
}
