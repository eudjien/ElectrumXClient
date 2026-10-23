using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.scripthash.get_history.</summary>
public sealed class ElectrumXHistoryQuery : ElectrumXQuery
{
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

    public string ScriptHash { get; }

    public long FromHeight { get; }

    public long ToHeight { get; }
}
