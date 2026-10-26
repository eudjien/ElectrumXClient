namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>Protocol values used by ElectrumX JSON properties and positional parameters.</summary>
public static class ElectrumXJsonPropertyValues
{
    public const string JsonRpcVersion = "2.0";

    public static class Transaction
    {
        public const bool Raw = false;
        public const bool Verbose = true;
    }

    public static class ServerFeatures
    {
        public const string HashFunctionSha256 = "sha256";
    }

    public static class TokenFilter
    {
        public const string IncludeTokens = "include_tokens";
        public const string ExcludeTokens = "exclude_tokens";
        public const string TokensOnly = "tokens_only";
    }

    public static class TscMerkle
    {
        public static class TransactionOrId
        {
            public const string TransactionId = "txid";
            public const string TransactionHash = "tx";
        }

        public static class TargetType
        {
            public const string BlockHash = "block_hash";
            public const string BlockHeader = "block_header";
            public const string MerkleRoot = "merkle_root";
        }
    }
}
