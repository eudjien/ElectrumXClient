using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>Default values for ElectrumX JSON-RPC method arguments.</summary>
public static class ElectrumXJsonMethodArgumentDefaultValues
{
    public static class ElectrumXAddressBalanceQuery
    {
        public static readonly ElectrumXTokenFilter? TokenFilter = null;
    }

    public static class ElectrumXAddressHistoryQuery
    {
        public const long FromHeight = 0;
        public const long ToHeight = -1;
    }

    public static class ElectrumXAddressListUnspentQuery
    {
        public static readonly ElectrumXTokenFilter? TokenFilter = null;
    }

    public static class ElectrumXBalanceQuery
    {
        public static readonly ElectrumXTokenFilter? TokenFilter = null;
    }

    public static class ElectrumXBlockHeadersQuery
    {
        public const int CheckpointHeight = 0;
    }

    public static class ElectrumXBroadcastPackageQuery
    {
        public const bool Verbose = false;
    }

    public static class ElectrumXConfirmedTransactionBlockHashQuery
    {
        public const bool IncludeHeader = false;
    }

    public static class ElectrumXEstimateFeeQuery
    {
        public const string? Mode = null;
    }

    public static class ElectrumXHistoryQuery
    {
        public const long FromHeight = 0;
        public const long ToHeight = -1;
    }

    public static class ElectrumXListUnspentQuery
    {
        public static readonly ElectrumXTokenFilter? TokenFilter = null;
    }

    public static class ElectrumXRpaHistoryQuery
    {
        public const long ToHeight = -1;
    }

    public static class ElectrumXSubscribeToHeadersQuery
    {
        public static readonly bool? Raw = null;
    }

    public static class ElectrumXTransactionIdFromPositionQuery
    {
        public const bool Merkle = false;
    }

    public static class ElectrumXTransactionMerkleQuery
    {
        public static readonly long? Height = null;
    }

    public static class ElectrumXTscMerkleWithTransactionHashQuery
    {
        public const TscMerkleTargetType TargetType = TscMerkleTargetType.BlockHash;
    }

    public static class ElectrumXTscMerkleWithTransactionHexQuery
    {
        public const TscMerkleTargetType TargetType = TscMerkleTargetType.BlockHash;
    }

    public static class ElectrumXVerboseTransactionQuery
    {
        public const bool Verbose = false;
    }
}
