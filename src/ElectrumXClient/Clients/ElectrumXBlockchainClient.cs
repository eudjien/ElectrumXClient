using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Clients;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using NorthGomel.ElectrumXClient.JsonRpc;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ElectrumXRpcClientOptions=NorthGomel.ElectrumXClient.JsonRpc.ElectrumXRpcClientOptions;

namespace NorthGomel.ElectrumXClient.Clients;

/// <inheritdoc />
public sealed class ElectrumXBlockchainClient : ElectrumXClientBase, IElectrumXBlockchainClient
{
    internal ElectrumXBlockchainClient(IElectrumXRpcClient rpcClient, bool ownsRpcClient = true)
        : base(rpcClient, ownsRpcClient)
    {
        RpcClient.BlockHeaderReceived += HandleBlockHeader;
        RpcClient.BlockHeightReceived += HandleBlockHeight;
        RpcClient.ScriptHashStatusChanged += HandleScriptHashStatus;
        RpcClient.AddressStatusChanged += HandleAddressStatusChanged;
        RpcClient.TransactionStatusChanged += HandleTransactionStatusChanged;
        RpcClient.DoubleSpendProofStatusChanged += HandleDoubleSpendProofStatusChanged;
    }

    /// <inheritdoc />
    public event EventHandler<ElectrumXBlockHeaderEventArgs>? BlockHeaderReceived;

    /// <inheritdoc />
    public event EventHandler<ElectrumXBlockHeightEventArgs>? BlockHeightReceived;

    /// <inheritdoc />
    public event EventHandler<ElectrumXScriptHashStatusEventArgs>? ScriptHashStatusChanged;

    /// <inheritdoc />
    public event EventHandler<ElectrumXAddressNotificationEventArgs>? AddressStatusChanged;

    /// <inheritdoc />
    public event EventHandler<ElectrumXTransactionNotificationEventArgs>? TransactionStatusChanged;

    /// <inheritdoc />
    public event EventHandler<ElectrumXDoubleSpendProofNotificationEventArgs>? DoubleSpendProofStatusChanged;

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        RpcClient.BlockHeaderReceived -= HandleBlockHeader;
        RpcClient.BlockHeightReceived -= HandleBlockHeight;
        RpcClient.ScriptHashStatusChanged -= HandleScriptHashStatus;
        RpcClient.AddressStatusChanged -= HandleAddressStatusChanged;
        RpcClient.TransactionStatusChanged -= HandleTransactionStatusChanged;
        RpcClient.DoubleSpendProofStatusChanged -= HandleDoubleSpendProofStatusChanged;
        await base.DisposeAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBlockHeaders> GetBlockHeadersAsync(
        long startHeight,
        int count,
        long checkpointHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBlockHeadersQuery.CheckpointHeight,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version120, ElectrumXJsonMethodNames.Blockchain.Block.Headers);
        if (checkpointHeight != ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBlockHeadersQuery.CheckpointHeight)
        {
            EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version140, ElectrumXJsonMethodNames.Blockchain.Block.Headers);
        }

        return await CallOrThrowAsync<ElectrumXBlockHeaders, ElectrumXBlockHeadersQuery>(
            new JsonRpcRequest<ElectrumXBlockHeadersQuery>(
                ElectrumXJsonMethodNames.Blockchain.Block.Headers,
                new ElectrumXBlockHeadersQuery(startHeight, count, checkpointHeight)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBlockHeaderChunk> GetBlockHeaderChunkAsync(
        long chunkIndex,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolVersionRange(
            ElectrumXProtocolVersions.Version100,
            ElectrumXProtocolVersions.Version140,
            ElectrumXJsonMethodNames.Blockchain.Block.GetChunk);
        return await CallOrThrowAsync<ElectrumXBlockHeaderChunk, ElectrumXBlockHeaderChunkQuery>(
            new JsonRpcRequest<ElectrumXBlockHeaderChunkQuery>(
                ElectrumXJsonMethodNames.Blockchain.Block.GetChunk,
                new ElectrumXBlockHeaderChunkQuery(chunkIndex)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXJsonValueResult> GetLegacyBlockHeaderAsync(
        long height,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolVersionRange(
            ElectrumXProtocolVersions.Version100,
            ElectrumXProtocolVersions.Version140,
            ElectrumXJsonMethodNames.Blockchain.Block.GetHeader);
        return await CallOrThrowAsync<ElectrumXJsonValueResult, ElectrumXLegacyBlockHeaderQuery>(
            new JsonRpcRequest<ElectrumXLegacyBlockHeaderQuery>(
                ElectrumXJsonMethodNames.Blockchain.Block.GetHeader,
                new ElectrumXLegacyBlockHeaderQuery(height)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXUtxoAddress> GetUtxoAddressAsync(
        string transactionHash,
        int outputIndex,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolVersionRange(
            ElectrumXProtocolVersions.Version100,
            ElectrumXProtocolVersions.Version110,
            ElectrumXJsonMethodNames.Blockchain.Utxo.GetAddress);
        return await CallOrThrowAsync<ElectrumXUtxoAddress, ElectrumXUtxoAddressQuery>(
            new JsonRpcRequest<ElectrumXUtxoAddressQuery>(
                ElectrumXJsonMethodNames.Blockchain.Utxo.GetAddress,
                new ElectrumXUtxoAddressQuery(transactionHash, outputIndex)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBlockHeight> SubscribeToBlockHeightAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolVersionRange(
            ElectrumXProtocolVersions.Version100,
            ElectrumXProtocolVersions.Version110,
            ElectrumXJsonMethodNames.Blockchain.NumBlocks.Subscribe);
        return await CallOrThrowAsync<ElectrumXBlockHeight, ElectrumXNumBlocksSubscribeQuery>(
            new JsonRpcRequest<ElectrumXNumBlocksSubscribeQuery>(
                ElectrumXJsonMethodNames.Blockchain.NumBlocks.Subscribe,
                new ElectrumXNumBlocksSubscribeQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<ElectrumXEstimateFee> EstimateFeeAsync(
        int numberOfBlocks,
        CancellationToken cancellationToken = default)
    {
        return EstimateFeeAsync(numberOfBlocks, null, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ElectrumXEstimateFee> EstimateFeeAsync(
        int numberOfBlocks,
        string? mode,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Blockchain.EstimateFee);
        if (mode is not null)
        {
            EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version160, ElectrumXJsonMethodNames.Blockchain.EstimateFee);
        }

        return await CallOrThrowAsync<ElectrumXEstimateFee, ElectrumXEstimateFeeQuery>(
            new JsonRpcRequest<ElectrumXEstimateFeeQuery>(
                ElectrumXJsonMethodNames.Blockchain.EstimateFee,
                new ElectrumXEstimateFeeQuery(numberOfBlocks, mode)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBlockHeaderSubscription> SubscribeToHeadersAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Blockchain.HeadersSubscribe);
        var protocolVersion = ServerVersion.ProtocolVersion;
        bool? raw = protocolVersion >= ElectrumXProtocolVersions.Version120 &&
                    protocolVersion < ElectrumXProtocolVersions.Version130
            ? true
            : null;

        return await CallOrThrowAsync<ElectrumXBlockHeaderSubscription, ElectrumXSubscribeToHeadersQuery>(
            new JsonRpcRequest<ElectrumXSubscribeToHeadersQuery>(
                ElectrumXJsonMethodNames.Blockchain.HeadersSubscribe,
                new ElectrumXSubscribeToHeadersQuery(raw)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXRelayFee> GetRelayFeeAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Blockchain.RelayFee);
        return await CallOrThrowAsync<ElectrumXRelayFee, ElectrumXRelayFeeQuery>(
            new JsonRpcRequest<ElectrumXRelayFeeQuery>(
                ElectrumXJsonMethodNames.Blockchain.RelayFee,
                new ElectrumXRelayFeeQuery()),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBalance> GetBalanceAsync(string scriptHash, CancellationToken cancellationToken = default)
    {
        return await GetBalanceAsync(scriptHash, null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBalance> GetBalanceAsync(string scriptHash, string? tokenFilter, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version110, ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetBalance);
        if (tokenFilter is not null)
        {
            EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version150, ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetBalance);
        }

        return await CallOrThrowAsync<ElectrumXBalance, ElectrumXBalanceQuery>(
            new JsonRpcRequest<ElectrumXBalanceQuery>(
                ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetBalance,
                new ElectrumXBalanceQuery(scriptHash, tokenFilter)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXHistory> GetHistoryAsync(string scriptHash, CancellationToken cancellationToken = default)
    {
        return await GetHistoryAsync(
            scriptHash,
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.FromHeight,
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.ToHeight,
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXHistory> GetHistoryAsync(
        string scriptHash,
        long fromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.ToHeight,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version110, ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetHistory);
        if (fromHeight != ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.FromHeight ||
            toHeight != ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.ToHeight)
        {
            EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version151, ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetHistory);
        }

        return await CallOrThrowAsync<ElectrumXHistory, ElectrumXHistoryQuery>(
            new JsonRpcRequest<ElectrumXHistoryQuery>(
                ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetHistory,
                new ElectrumXHistoryQuery(scriptHash, fromHeight, toHeight)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXMempool> GetMempoolAsync(string scriptHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version110, ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetMempool);
        return await CallOrThrowAsync<ElectrumXMempool, ElectrumXMempoolQuery>(
            new JsonRpcRequest<ElectrumXMempoolQuery>(
                ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetMempool,
                new ElectrumXMempoolQuery(scriptHash)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXUnspentOutputs> ListUnspentAsync(string scriptHash, CancellationToken cancellationToken = default)
    {
        return await ListUnspentAsync(scriptHash, null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXUnspentOutputs> ListUnspentAsync(string scriptHash, string? tokenFilter, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version110, ElectrumXJsonMethodNames.Blockchain.ScriptHash.ListUnspent);
        if (tokenFilter is not null)
        {
            EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version150, ElectrumXJsonMethodNames.Blockchain.ScriptHash.ListUnspent);
        }

        return await CallOrThrowAsync<ElectrumXUnspentOutputs, ElectrumXListUnspentQuery>(
            new JsonRpcRequest<ElectrumXListUnspentQuery>(
                ElectrumXJsonMethodNames.Blockchain.ScriptHash.ListUnspent,
                new ElectrumXListUnspentQuery(scriptHash, tokenFilter)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBlockHeader> GetBlockHeaderAsync(long height, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version130, ElectrumXJsonMethodNames.Blockchain.Block.Header);
        return await CallOrThrowAsync<ElectrumXBlockHeader, ElectrumXBlockHeaderQuery>(
            new JsonRpcRequest<ElectrumXBlockHeaderQuery>(
                ElectrumXJsonMethodNames.Blockchain.Block.Header,
                new ElectrumXBlockHeaderQuery(height)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBlockHeaderProof> GetBlockHeaderWithProofAsync(long height, long checkpointHeight,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version140, ElectrumXJsonMethodNames.Blockchain.Block.Header);
        return await CallOrThrowAsync<ElectrumXBlockHeaderProof, ElectrumXBlockHeaderWithProofQuery>(
            new JsonRpcRequest<ElectrumXBlockHeaderWithProofQuery>(
                ElectrumXJsonMethodNames.Blockchain.Block.Header,
                new ElectrumXBlockHeaderWithProofQuery(height, checkpointHeight)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXScriptHashSubscription> SubscribeToScriptHashAsync(string scriptHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version110, ElectrumXJsonMethodNames.Blockchain.ScriptHash.Subscribe);
        return await CallOrThrowAsync<ElectrumXScriptHashSubscription, ElectrumXSubscribeToScriptHashQuery>(
            new JsonRpcRequest<ElectrumXSubscribeToScriptHashQuery>(
                ElectrumXJsonMethodNames.Blockchain.ScriptHash.Subscribe,
                new ElectrumXSubscribeToScriptHashQuery(scriptHash)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXScriptHashUnsubscription> UnsubscribeFromScriptHashAsync(string scriptHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version142, ElectrumXJsonMethodNames.Blockchain.ScriptHash.Unsubscribe);
        return await CallOrThrowAsync<ElectrumXScriptHashUnsubscription, ElectrumXUnsubscribeFromScriptHashQuery>(
            new JsonRpcRequest<ElectrumXUnsubscribeFromScriptHashQuery>(
                ElectrumXJsonMethodNames.Blockchain.ScriptHash.Unsubscribe,
                new ElectrumXUnsubscribeFromScriptHashQuery(scriptHash)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXRawTransaction> GetRawTransactionAsync(string transactionHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Blockchain.Transaction.Get);
        return await CallOrThrowAsync<ElectrumXRawTransaction, ElectrumXRawTransactionQuery>(
            new JsonRpcRequest<ElectrumXRawTransactionQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.Get,
                new ElectrumXRawTransactionQuery(transactionHash)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXVerboseTransaction> GetVerboseTransactionAsync(string transactionHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version120, ElectrumXJsonMethodNames.Blockchain.Transaction.Get);
        return await CallOrThrowAsync<ElectrumXVerboseTransaction, ElectrumXVerboseTransactionQuery>(
            new JsonRpcRequest<ElectrumXVerboseTransactionQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.Get,
                new ElectrumXVerboseTransactionQuery(transactionHash)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBroadcastTransaction> BroadcastTransactionAsync(string rawTransaction, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Blockchain.Transaction.Broadcast);
        return await CallOrThrowAsync<ElectrumXBroadcastTransaction, ElectrumXBroadcastTransactionQuery>(
            new JsonRpcRequest<ElectrumXBroadcastTransactionQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.Broadcast,
                new ElectrumXBroadcastTransactionQuery(rawTransaction)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXJsonValueResult> BroadcastTransactionPackageAsync(
        IReadOnlyList<string> rawTransactions,
        bool verbose = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBroadcastPackageQuery.Verbose,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version160, ElectrumXJsonMethodNames.Blockchain.Transaction.BroadcastPackage);

        return await CallOrThrowAsync<ElectrumXJsonValueResult, ElectrumXBroadcastPackageQuery>(
            new JsonRpcRequest<ElectrumXBroadcastPackageQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.BroadcastPackage,
                new ElectrumXBroadcastPackageQuery(rawTransactions, verbose)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXTransactionMerkle> GetTransactionMerkleAsync(
        string transactionHash,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version145, ElectrumXJsonMethodNames.Blockchain.Transaction.GetMerkle);
        return await CallOrThrowAsync<ElectrumXTransactionMerkle, ElectrumXTransactionMerkleQuery>(
            new JsonRpcRequest<ElectrumXTransactionMerkleQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.GetMerkle,
                new ElectrumXTransactionMerkleQuery(transactionHash)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXTransactionMerkle> GetTransactionMerkleAsync(string transactionHash, long height,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version100, ElectrumXJsonMethodNames.Blockchain.Transaction.GetMerkle);
        return await CallOrThrowAsync<ElectrumXTransactionMerkle, ElectrumXTransactionMerkleQuery>(
            new JsonRpcRequest<ElectrumXTransactionMerkleQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.GetMerkle,
                new ElectrumXTransactionMerkleQuery(
                    transactionHash,
                    height)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXTscMerkleWithTransactionHash> GetTscMerkleWithTransactionHashAsync(
        string transactionHash,
        long height,
        TscMerkleTargetType targetType =
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXTscMerkleWithTransactionHashQuery.TargetType,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        return await CallOrThrowAsync<ElectrumXTscMerkleWithTransactionHash, ElectrumXTscMerkleWithTransactionHashQuery>(
            new JsonRpcRequest<ElectrumXTscMerkleWithTransactionHashQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.GetTscMerkle,
                new ElectrumXTscMerkleWithTransactionHashQuery(
                    transactionHash,
                    height,
                    targetType)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXTscMerkleWithTransactionHex> GetTscMerkleWithTransactionHexAsync(string transactionHash, long height,
        TscMerkleTargetType targetType =
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXTscMerkleWithTransactionHexQuery.TargetType,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        return await CallOrThrowAsync<ElectrumXTscMerkleWithTransactionHex, ElectrumXTscMerkleWithTransactionHexQuery>(
            new JsonRpcRequest<ElectrumXTscMerkleWithTransactionHexQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.GetTscMerkle,
                new ElectrumXTscMerkleWithTransactionHexQuery(
                    transactionHash,
                    height,
                    targetType)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXIdFromPosition> GetTransactionIdFromPositionAsync(
        long height,
        int transactionPosition,
        bool merkle = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXTransactionIdFromPositionQuery.Merkle,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version140, ElectrumXJsonMethodNames.Blockchain.Transaction.IdFromPosition);

        return await CallOrThrowAsync<ElectrumXIdFromPosition, ElectrumXTransactionIdFromPositionQuery>(
            new JsonRpcRequest<ElectrumXTransactionIdFromPositionQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.IdFromPosition,
                new ElectrumXTransactionIdFromPositionQuery(height, transactionPosition, merkle)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXBalance> GetAddressBalanceAsync(
        string address,
        string? tokenFilter = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressBalanceQuery.TokenFilter,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureAddressCompatibilityMethod(ElectrumXJsonMethodNames.Blockchain.Address.GetBalance);
        if (tokenFilter is not null)
        {
            EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version150, ElectrumXJsonMethodNames.Blockchain.Address.GetBalance);
        }

        return await CallOrThrowAsync<ElectrumXBalance, ElectrumXAddressBalanceQuery>(
            new JsonRpcRequest<ElectrumXAddressBalanceQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.GetBalance,
                new ElectrumXAddressBalanceQuery(address, tokenFilter)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXHistory> GetAddressHistoryAsync(
        string address,
        long fromHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.FromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.ToHeight,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureAddressCompatibilityMethod(ElectrumXJsonMethodNames.Blockchain.Address.GetHistory);
        if (fromHeight != ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.FromHeight ||
            toHeight != ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.ToHeight)
        {
            EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version151, ElectrumXJsonMethodNames.Blockchain.Address.GetHistory);
        }

        return await CallOrThrowAsync<ElectrumXHistory, ElectrumXAddressHistoryQuery>(
            new JsonRpcRequest<ElectrumXAddressHistoryQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.GetHistory,
                new ElectrumXAddressHistoryQuery(address, fromHeight, toHeight)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXMempool> GetAddressMempoolAsync(string address, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureAddressCompatibilityMethod(ElectrumXJsonMethodNames.Blockchain.Address.GetMempool);

        return await CallOrThrowAsync<ElectrumXMempool, ElectrumXAddressMempoolQuery>(
            new JsonRpcRequest<ElectrumXAddressMempoolQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.GetMempool,
                new ElectrumXAddressMempoolQuery(address)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ElectrumXUnspentOutputs> ListAddressUnspentAsync(
        string address,
        string? tokenFilter = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressListUnspentQuery.TokenFilter,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureAddressCompatibilityMethod(ElectrumXJsonMethodNames.Blockchain.Address.ListUnspent);
        if (tokenFilter is not null)
        {
            EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version150, ElectrumXJsonMethodNames.Blockchain.Address.ListUnspent);
        }

        return await CallOrThrowAsync<ElectrumXUnspentOutputs, ElectrumXAddressListUnspentQuery>(
            new JsonRpcRequest<ElectrumXAddressListUnspentQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.ListUnspent,
                new ElectrumXAddressListUnspentQuery(address, tokenFilter)),
            cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<ElectrumXAddressScriptHash> GetAddressScriptHashAsync(string address, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version143, ElectrumXJsonMethodNames.Blockchain.Address.GetScriptHash);
        return CallOrThrowAsync<ElectrumXAddressScriptHash, ElectrumXAddressScriptHashQuery>(
            new JsonRpcRequest<ElectrumXAddressScriptHashQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.GetScriptHash,
                new ElectrumXAddressScriptHashQuery(address)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXFirstUse?> GetAddressFirstUseAsync(string address, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version152, ElectrumXJsonMethodNames.Blockchain.Address.GetFirstUse);
        return CallAsync<ElectrumXFirstUse, ElectrumXAddressFirstUseQuery>(
            new JsonRpcRequest<ElectrumXAddressFirstUseQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.GetFirstUse,
                new ElectrumXAddressFirstUseQuery(address)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXAddressStatus> GetAddressStatusAsync(string address, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version160, ElectrumXJsonMethodNames.Blockchain.Address.GetStatus);
        return CallOrThrowAsync<ElectrumXAddressStatus, ElectrumXAddressStatusQuery>(
            new JsonRpcRequest<ElectrumXAddressStatusQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.GetStatus,
                new ElectrumXAddressStatusQuery(address)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXAddressSubscription> SubscribeToAddressAsync(string address, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureAddressCompatibilityMethod(ElectrumXJsonMethodNames.Blockchain.Address.Subscribe);
        return CallOrThrowAsync<ElectrumXAddressSubscription, ElectrumXSubscribeToAddressQuery>(
            new JsonRpcRequest<ElectrumXSubscribeToAddressQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.Subscribe,
                new ElectrumXSubscribeToAddressQuery(address)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXAddressUnsubscription> UnsubscribeFromAddressAsync(string address, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version143, ElectrumXJsonMethodNames.Blockchain.Address.Unsubscribe);
        return CallOrThrowAsync<ElectrumXAddressUnsubscription, ElectrumXUnsubscribeFromAddressQuery>(
            new JsonRpcRequest<ElectrumXUnsubscribeFromAddressQuery>(
                ElectrumXJsonMethodNames.Blockchain.Address.Unsubscribe,
                new ElectrumXUnsubscribeFromAddressQuery(address)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXUtxoInfo?> GetUnspentOutputInfoAsync(
        string transactionHash,
        int outputIndex,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version144, ElectrumXJsonMethodNames.Blockchain.Utxo.GetInfo);
        return CallAsync<ElectrumXUtxoInfo, ElectrumXUnspentOutputInfoQuery>(
            new JsonRpcRequest<ElectrumXUnspentOutputInfoQuery>(
                ElectrumXJsonMethodNames.Blockchain.Utxo.GetInfo,
                new ElectrumXUnspentOutputInfoQuery(transactionHash, outputIndex)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXTransactionHeightResult> GetTransactionHeightAsync(string transactionHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version145, ElectrumXJsonMethodNames.Blockchain.Transaction.GetHeight);
        return CallOrThrowAsync<ElectrumXTransactionHeightResult, ElectrumXTransactionHeightQuery>(
            new JsonRpcRequest<ElectrumXTransactionHeightQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.GetHeight,
                new ElectrumXTransactionHeightQuery(transactionHash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXTransactionSubscription> SubscribeToTransactionAsync(
        string transactionHash,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version145, ElectrumXJsonMethodNames.Blockchain.Transaction.Subscribe);
        return CallOrThrowAsync<ElectrumXTransactionSubscription, ElectrumXSubscribeToTransactionQuery>(
            new JsonRpcRequest<ElectrumXSubscribeToTransactionQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.Subscribe,
                new ElectrumXSubscribeToTransactionQuery(transactionHash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXTransactionUnsubscription> UnsubscribeFromTransactionAsync(
        string transactionHash,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version145, ElectrumXJsonMethodNames.Blockchain.Transaction.Unsubscribe);
        return CallOrThrowAsync<ElectrumXTransactionUnsubscription, ElectrumXUnsubscribeFromTransactionQuery>(
            new JsonRpcRequest<ElectrumXUnsubscribeFromTransactionQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.Unsubscribe,
                new ElectrumXUnsubscribeFromTransactionQuery(transactionHash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXDoubleSpendProof?> GetDoubleSpendProofAsync(
        string hash,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version145, ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.Get);
        return CallAsync<ElectrumXDoubleSpendProof, ElectrumXGetDoubleSpendProofQuery>(
            new JsonRpcRequest<ElectrumXGetDoubleSpendProofQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.Get,
                new ElectrumXGetDoubleSpendProofQuery(hash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXStringList> GetDoubleSpendProofTransactionsAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version145, ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.List);
        return CallOrThrowAsync<ElectrumXStringList, ElectrumXListDoubleSpendProofTransactionsQuery>(
            new JsonRpcRequest<ElectrumXListDoubleSpendProofTransactionsQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.List,
                new ElectrumXListDoubleSpendProofTransactionsQuery()),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXDoubleSpendProof?> SubscribeToDoubleSpendProofAsync(
        string transactionHash,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version145, ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.Subscribe);
        return CallAsync<ElectrumXDoubleSpendProof, ElectrumXSubscribeToDoubleSpendProofQuery>(
            new JsonRpcRequest<ElectrumXSubscribeToDoubleSpendProofQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.Subscribe,
                new ElectrumXSubscribeToDoubleSpendProofQuery(transactionHash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXDoubleSpendProofUnsubscription> UnsubscribeFromDoubleSpendProofAsync(
        string transactionHash,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version145, ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.Unsubscribe);
        return CallOrThrowAsync<ElectrumXDoubleSpendProofUnsubscription, ElectrumXUnsubscribeFromDoubleSpendProofQuery>(
            new JsonRpcRequest<ElectrumXUnsubscribeFromDoubleSpendProofQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.Unsubscribe,
                new ElectrumXUnsubscribeFromDoubleSpendProofQuery(transactionHash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXConfirmedTransactionBlockHash> GetConfirmedTransactionBlockHashAsync(
        string transactionHash,
        bool includeHeader =
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXConfirmedTransactionBlockHashQuery.IncludeHeader,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version152, ElectrumXJsonMethodNames.Blockchain.Transaction.GetConfirmedBlockHash);
        return CallOrThrowAsync<ElectrumXConfirmedTransactionBlockHash, ElectrumXConfirmedTransactionBlockHashQuery>(
            new JsonRpcRequest<ElectrumXConfirmedTransactionBlockHashQuery>(
                ElectrumXJsonMethodNames.Blockchain.Transaction.GetConfirmedBlockHash,
                new ElectrumXConfirmedTransactionBlockHashQuery(transactionHash, includeHeader)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXHeaderInfo> GetHeaderAsync(string blockHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version152, ElectrumXJsonMethodNames.Blockchain.Header.Get);
        return CallOrThrowAsync<ElectrumXHeaderInfo, ElectrumXHeaderByHashQuery>(
            new JsonRpcRequest<ElectrumXHeaderByHashQuery>(
                ElectrumXJsonMethodNames.Blockchain.Header.Get,
                new ElectrumXHeaderByHashQuery(blockHash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXHeaderInfo> GetHeaderAsync(long height, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version152, ElectrumXJsonMethodNames.Blockchain.Header.Get);
        return CallOrThrowAsync<ElectrumXHeaderInfo, ElectrumXHeaderByHeightQuery>(
            new JsonRpcRequest<ElectrumXHeaderByHeightQuery>(
                ElectrumXJsonMethodNames.Blockchain.Header.Get,
                new ElectrumXHeaderByHeightQuery(height)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXHeaderInfo> GetHeaderTipAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version150, ElectrumXJsonMethodNames.Blockchain.Headers.GetTip);
        return CallOrThrowAsync<ElectrumXHeaderInfo, ElectrumXGetHeaderTipQuery>(
            new JsonRpcRequest<ElectrumXGetHeaderTipQuery>(
                ElectrumXJsonMethodNames.Blockchain.Headers.GetTip,
                new ElectrumXGetHeaderTipQuery()),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXHeadersUnsubscription> UnsubscribeFromHeadersAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version150, ElectrumXJsonMethodNames.Blockchain.Headers.Unsubscribe);
        return CallOrThrowAsync<ElectrumXHeadersUnsubscription, ElectrumXUnsubscribeFromHeadersQuery>(
            new JsonRpcRequest<ElectrumXUnsubscribeFromHeadersQuery>(
                ElectrumXJsonMethodNames.Blockchain.Headers.Unsubscribe,
                new ElectrumXUnsubscribeFromHeadersQuery()),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXScriptHashStatus> GetScriptHashStatusAsync(string scriptHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version160, ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetStatus);
        return CallOrThrowAsync<ElectrumXScriptHashStatus, ElectrumXScriptHashStatusQuery>(
            new JsonRpcRequest<ElectrumXScriptHashStatusQuery>(
                ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetStatus,
                new ElectrumXScriptHashStatusQuery(scriptHash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXFirstUse?> GetScriptHashFirstUseAsync(string scriptHash, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version152, ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetFirstUse);
        return CallAsync<ElectrumXFirstUse, ElectrumXScriptHashFirstUseQuery>(
            new JsonRpcRequest<ElectrumXScriptHashFirstUseQuery>(
                ElectrumXJsonMethodNames.Blockchain.ScriptHash.GetFirstUse,
                new ElectrumXScriptHashFirstUseQuery(scriptHash)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXHistoryList> GetRpaHistoryAsync(
        string rpaPrefix,
        long fromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXRpaHistoryQuery.ToHeight,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version153, ElectrumXJsonMethodNames.Blockchain.Rpa.GetHistory);
        return CallOrThrowAsync<ElectrumXHistoryList, ElectrumXRpaHistoryQuery>(
            new JsonRpcRequest<ElectrumXRpaHistoryQuery>(
                ElectrumXJsonMethodNames.Blockchain.Rpa.GetHistory,
                new ElectrumXRpaHistoryQuery(rpaPrefix, fromHeight, toHeight)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXMempoolEntryList> GetRpaMempoolAsync(string rpaPrefix,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version153, ElectrumXJsonMethodNames.Blockchain.Rpa.GetMempool);
        return CallOrThrowAsync<ElectrumXMempoolEntryList, ElectrumXRpaMempoolQuery>(
            new JsonRpcRequest<ElectrumXRpaMempoolQuery>(
                ElectrumXJsonMethodNames.Blockchain.Rpa.GetMempool,
                new ElectrumXRpaMempoolQuery(rpaPrefix)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXHistoryList> GetReusableHistoryAsync(
        long fromHeight,
        int count,
        string rpaPrefix,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version153, ElectrumXJsonMethodNames.Blockchain.Reusable.GetHistory);
        return CallOrThrowAsync<ElectrumXHistoryList, ElectrumXReusableHistoryQuery>(
            new JsonRpcRequest<ElectrumXReusableHistoryQuery>(
                ElectrumXJsonMethodNames.Blockchain.Reusable.GetHistory,
                new ElectrumXReusableHistoryQuery(fromHeight, count, rpaPrefix)),
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ElectrumXMempoolEntryList> GetReusableMempoolAsync(
        string rpaPrefix,
        CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        EnsureProtocolMinimumVersion(ElectrumXProtocolVersions.Version153, ElectrumXJsonMethodNames.Blockchain.Reusable.GetMempool);
        return CallOrThrowAsync<ElectrumXMempoolEntryList, ElectrumXReusableMempoolQuery>(
            new JsonRpcRequest<ElectrumXReusableMempoolQuery>(
                ElectrumXJsonMethodNames.Blockchain.Reusable.GetMempool,
                new ElectrumXReusableMempoolQuery(rpaPrefix)),
            cancellationToken);
    }

    /// <inheritdoc />
    public override ElectrumXServerVersion ServerVersion => RpcClient.ServerVersion;

    private void EnsureAddressCompatibilityMethod(string methodName)
    {
        var version = ServerVersion.ProtocolVersion;
        if (version >= ElectrumXProtocolVersions.Version100 && version < ElectrumXProtocolVersions.Version130 ||
            version >= ElectrumXProtocolVersions.Version143)
        {
            return;
        }

        throw new NotSupportedException(
            $"{methodName} is available for protocol versions 1.0-1.2 and 1.4.3 or later; the negotiated version is {version}.");
    }

    private void EnsureProtocolVersionRange(
        ElectrumXProtocolVersion minimumVersion,
        ElectrumXProtocolVersion maximumExclusive,
        string methodName)
    {
        var version = ServerVersion.ProtocolVersion;
        if (version >= minimumVersion && version < maximumExclusive)
        {
            return;
        }

        throw new NotSupportedException(
            $"{methodName} is available from protocol version {minimumVersion} until {maximumExclusive}; the negotiated version is {version}.");
    }

    /// <summary>Connects a blockchain API client using the supplied JSON-RPC transport.</summary>
    /// <param name="rpcClient">The JSON-RPC transport to connect and use; must not be <see langword="null" />.</param>
    /// <param name="ownsRpcClient">Whether disposing this client also disposes <paramref name="rpcClient" />; defaults to <see langword="true" />.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected blockchain API client.</returns>
    public static async Task<ElectrumXBlockchainClient> ConnectAsync(
        IElectrumXRpcClient rpcClient,
        bool ownsRpcClient = true,
        CancellationToken cancellationToken = default)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(rpcClient, nameof(rpcClient));
        var client = new ElectrumXBlockchainClient(rpcClient, ownsRpcClient);

        try
        {
            if (!rpcClient.IsConnected)
            {
                await rpcClient.ConnectAsync(cancellationToken).ConfigureAwait(false);
            }

            return client;
        }
        catch
        {
            await client.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    /// <summary>Creates and connects a blockchain client using unencrypted TCP.</summary>
    /// <param name="host">The server host name or IP address.</param>
    /// <param name="port">The server port.</param>
    /// <param name="cancellationToken">Token used to cancel connection and protocol negotiation.</param>
    /// <returns>A connected blockchain API client.</returns>
    public static Task<ElectrumXBlockchainClient> ConnectAsync(
        string host,
        int port,
        CancellationToken cancellationToken = default)
    {
        return ConnectAsync(
            new ElectrumXTcpRpcClient(new ElectrumXRpcClientOptions
            {
                Host = host, Port = port
            }),
            cancellationToken: cancellationToken);
    }

    private void HandleBlockHeader(object? sender, ElectrumXBlockHeaderEventArgs eventArgs)
    {
        ElectrumXNotificationPublisher.Publish(BlockHeaderReceived, this, eventArgs);
    }

    private void HandleBlockHeight(object? sender, ElectrumXBlockHeightEventArgs eventArgs)
    {
        ElectrumXNotificationPublisher.Publish(BlockHeightReceived, this, eventArgs);
    }

    private void HandleScriptHashStatus(object? sender, ElectrumXScriptHashStatusEventArgs eventArgs)
    {
        ElectrumXNotificationPublisher.Publish(ScriptHashStatusChanged, this, eventArgs);
    }

    private void HandleAddressStatusChanged(object? sender, ElectrumXAddressNotificationEventArgs eventArgs)
    {
        ElectrumXNotificationPublisher.Publish(AddressStatusChanged, this, eventArgs);
    }

    private void HandleTransactionStatusChanged(object? sender, ElectrumXTransactionNotificationEventArgs eventArgs)
    {
        ElectrumXNotificationPublisher.Publish(TransactionStatusChanged, this, eventArgs);
    }

    private void HandleDoubleSpendProofStatusChanged(
        object? sender,
        ElectrumXDoubleSpendProofNotificationEventArgs eventArgs)
    {
        ElectrumXNotificationPublisher.Publish(DoubleSpendProofStatusChanged, this, eventArgs);
    }
}
