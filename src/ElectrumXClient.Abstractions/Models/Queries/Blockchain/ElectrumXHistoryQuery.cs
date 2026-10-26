using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.get_history.</summary>
public sealed class ElectrumXHistoryQuery : ElectrumXQuery
{
    /// <summary>Creates a history query for a script hash and an optional height range.</summary>
    /// <param name="scriptHash">The script hash whose history is requested.</param>
    /// <param name="fromHeight">Inclusive first block height; defaults to zero.</param>
    /// <param name="toHeight">Exclusive end height, or <c>-1</c> to scan to the server's limit.</param>
    public ElectrumXHistoryQuery(
        string scriptHash,
        long fromHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.FromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.ToHeight)
    {
        if (fromHeight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fromHeight));
        }

        if (toHeight < ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.ToHeight ||
            toHeight != ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.ToHeight && fromHeight > toHeight)
        {
            throw new ArgumentOutOfRangeException(nameof(toHeight));
        }

        ScriptHash = ArgumentGuard.ValidateAndNormalizeSha256Hash(scriptHash, nameof(scriptHash));
        FromHeight = fromHeight;
        ToHeight = toHeight;
    }

    /// <summary>Gets the script hash whose history is requested.</summary>
    public string ScriptHash { get; }

    /// <summary>Gets the inclusive first block height to scan.</summary>
    public long FromHeight { get; }

    /// <summary>Gets the exclusive end height, or <c>-1</c> for the server-defined limit.</summary>
    public long ToHeight { get; }
}
