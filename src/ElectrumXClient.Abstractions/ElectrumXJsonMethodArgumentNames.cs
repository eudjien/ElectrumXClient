namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>ElectrumX JSON-RPC argument names.</summary>
public static class ElectrumXJsonMethodArgumentNames
{
    public static class ElectrumXAddressBalanceQuery
    {
        public const string TokenFilter = "token_filter";
    }

    public static class ElectrumXAddressHistoryQuery
    {
        public const string FromHeight = "from_height";
        public const string ToHeight = "to_height";
    }

    public static class ElectrumXAddressListUnspentQuery
    {
        public const string TokenFilter = "token_filter";
    }

    public static class ElectrumXBalanceQuery
    {
        public const string TokenFilter = "token_filter";
    }

    public static class ElectrumXBlockHeaderWithProofQuery
    {
        public const string CheckpointHeight = "checkpoint_height";
    }

    public static class ElectrumXBlockHeadersQuery
    {
        public const string CheckpointHeight = "checkpoint_height";
    }

    public static class ElectrumXEstimateFeeQuery
    {
        public const string Mode = "mode";
    }

    public static class ElectrumXHistoryQuery
    {
        public const string FromHeight = "from_height";
        public const string ToHeight = "to_height";
    }

    public static class ElectrumXListUnspentQuery
    {
        public const string TokenFilter = "token_filter";
    }

    public static class ElectrumXTransactionMerkleQuery
    {
        public const string Height = "height";
    }

    public static class ElectrumXVerboseTransactionQuery
    {
        public const string Verbose = "verbose";
    }
}
