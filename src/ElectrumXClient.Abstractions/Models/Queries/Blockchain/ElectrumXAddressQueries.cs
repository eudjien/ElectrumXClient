namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

public sealed class ElectrumXAddressBalanceQuery(
    string address,
    string? tokenFilter = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressBalanceQuery.TokenFilter)
    : ElectrumXQuery
{
    public string Address { get; } = address;

    public string? TokenFilter { get; } = tokenFilter;
}

public sealed class ElectrumXAddressHistoryQuery(
    string address,
    long fromHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.FromHeight,
    long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.ToHeight)
    : ElectrumXQuery
{
    public string Address { get; } = address;

    public long FromHeight { get; } = fromHeight;

    public long ToHeight { get; } = toHeight;
}

public sealed class ElectrumXAddressMempoolQuery(string address)
    : ElectrumXQuery
{
    public string Address { get; } = address;
}

public sealed class ElectrumXAddressListUnspentQuery(
    string address,
    string? tokenFilter = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressListUnspentQuery.TokenFilter)
    : ElectrumXQuery
{
    public string Address { get; } = address;

    public string? TokenFilter { get; } = tokenFilter;
}

public sealed class ElectrumXAddressScriptHashQuery(string address)
    : ElectrumXQuery
{
    public string Address { get; } = address;
}

public sealed class ElectrumXAddressFirstUseQuery(string address)
    : ElectrumXQuery
{
    public string Address { get; } = address;
}

public sealed class ElectrumXAddressStatusQuery(string address)
    : ElectrumXQuery
{
    public string Address { get; } = address;
}

public sealed class ElectrumXSubscribeToAddressQuery(string address)
    : ElectrumXQuery
{
    public string Address { get; } = address;
}

public sealed class ElectrumXUnsubscribeFromAddressQuery(string address)
    : ElectrumXQuery
{
    public string Address { get; } = address;
}

public sealed class ElectrumXUnspentOutputInfoQuery(string transactionHash, int outputIndex)
    : ElectrumXQuery
{
    public string TransactionHash { get; } = transactionHash;

    public int OutputIndex { get; } = outputIndex;
}

public sealed class ElectrumXTransactionHeightQuery(string transactionHash)
    : ElectrumXQuery
{
    public string TransactionHash { get; } = transactionHash;
}

public sealed class ElectrumXSubscribeToTransactionQuery(string transactionHash)
    : ElectrumXQuery
{
    public string TransactionHash { get; } = transactionHash;
}

public sealed class ElectrumXUnsubscribeFromTransactionQuery(string transactionHash)
    : ElectrumXQuery
{
    public string TransactionHash { get; } = transactionHash;
}

public sealed class ElectrumXGetDoubleSpendProofQuery(string hash)
    : ElectrumXQuery
{
    public string Hash { get; } = hash;
}

public sealed class ElectrumXListDoubleSpendProofTransactionsQuery
    : ElectrumXQuery;

public sealed class ElectrumXSubscribeToDoubleSpendProofQuery(string transactionHash)
    : ElectrumXQuery
{
    public string TransactionHash { get; } = transactionHash;
}

public sealed class ElectrumXUnsubscribeFromDoubleSpendProofQuery(string transactionHash)
    : ElectrumXQuery
{
    public string TransactionHash { get; } = transactionHash;
}

public sealed class ElectrumXConfirmedTransactionBlockHashQuery(
    string transactionHash,
    bool includeHeader = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXConfirmedTransactionBlockHashQuery.IncludeHeader)
    : ElectrumXQuery
{
    public string TransactionHash { get; } = transactionHash;

    public bool IncludeHeader { get; } = includeHeader;
}

public sealed class ElectrumXHeaderByHashQuery(string blockHash)
    : ElectrumXQuery
{
    public string BlockHash { get; } = blockHash;
}

public sealed class ElectrumXHeaderByHeightQuery(long height)
    : ElectrumXQuery
{
    public long Height { get; } = height;
}

public sealed class ElectrumXGetHeaderTipQuery
    : ElectrumXQuery;

public sealed class ElectrumXUnsubscribeFromHeadersQuery
    : ElectrumXQuery;

public sealed class ElectrumXScriptHashStatusQuery(string scriptHash)
    : ElectrumXQuery
{
    public string ScriptHash { get; } = scriptHash;
}

public sealed class ElectrumXScriptHashFirstUseQuery(string scriptHash)
    : ElectrumXQuery
{
    public string ScriptHash { get; } = scriptHash;
}

public sealed class ElectrumXRpaHistoryQuery(string rpaPrefix, long fromHeight, long toHeight)
    : ElectrumXQuery
{
    public string RpaPrefix { get; } = rpaPrefix;

    public long FromHeight { get; } = fromHeight;

    public long ToHeight { get; } = toHeight;
}

public sealed class ElectrumXRpaMempoolQuery(string rpaPrefix)
    : ElectrumXQuery
{
    public string RpaPrefix { get; } = rpaPrefix;
}

public sealed class ElectrumXReusableHistoryQuery(long fromHeight, int count, string rpaPrefix)
    : ElectrumXQuery
{
    public long FromHeight { get; } = fromHeight;

    public int Count { get; } = count;

    public string RpaPrefix { get; } = rpaPrefix;
}

public sealed class ElectrumXReusableMempoolQuery(string rpaPrefix)
    : ElectrumXQuery
{
    public string RpaPrefix { get; } = rpaPrefix;
}
