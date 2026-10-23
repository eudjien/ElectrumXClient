using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.Abstractions.Clients;

/// <summary>Provides operations for the blockchain-related subset of ElectrumX.</summary>
public interface IElectrumXBlockchainClient : IAsyncDisposable
{
    /// <summary>Server software and negotiated protocol version.</summary>
    /// <remarks>Negotiated using JSON-RPC method <c>server.version</c>.</remarks>
    ElectrumXServerVersion ServerVersion { get; }

    /// <summary>Occurs when a new block header is received from the server.</summary>
    event EventHandler<ElectrumXBlockHeaderEventArgs>? BlockHeaderReceived;

    /// <summary>Occurs when protocol version 1.0 reports a new block height.</summary>
    event EventHandler<ElectrumXBlockHeightEventArgs>? BlockHeightReceived;

    /// <summary>Occurs when a script hash status update is received from the server.</summary>
    event EventHandler<ElectrumXScriptHashStatusEventArgs>? ScriptHashStatusChanged;

    /// <summary>Occurs when an address status notification is received.</summary>
    event EventHandler<ElectrumXAddressNotificationEventArgs>? AddressStatusChanged;

    /// <summary>Occurs when a transaction notification is received.</summary>
    event EventHandler<ElectrumXTransactionNotificationEventArgs>? TransactionStatusChanged;

    /// <summary>Occurs when a double-spend-proof notification is received.</summary>
    event EventHandler<ElectrumXDoubleSpendProofNotificationEventArgs>? DoubleSpendProofStatusChanged;

    /// <summary>Returns a consecutive range of block headers from the main chain.</summary>
    /// <param name="startHeight">Zero-based height of the first header; must be non-negative.</param>
    /// <param name="count">Number of headers requested; must be non-negative. The server may return fewer headers or cap the result at its configured maximum.</param>
    /// <param name="checkpointHeight">
    ///     Checkpoint height used to request an inclusion proof. Must be non-negative; <c>0</c> (the default) disables the proof. When
    ///     positive and <paramref name="count" /> is positive, the requested final height must not exceed the checkpoint.
    /// </param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The returned headers, count, server limit, and, when requested, checkpoint proof data.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.block.headers</c>, available since protocol version 1.2. Checkpoint proofs were added in version 1.4. Results can
    ///     be shorter than requested when the chain is not tall enough.
    /// </remarks>
    Task<ElectrumXBlockHeaders> GetBlockHeadersAsync(
        long startHeight,
        int count,
        long checkpointHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBlockHeadersQuery.CheckpointHeight,
        CancellationToken cancellationToken = default);

    /// <summary>Returns a legacy concatenated chunk of block headers.</summary>
    /// <param name="chunkIndex">Zero-based chunk index.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The available serialized block headers in the chunk.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.block.get_chunk</c>, available before protocol version 1.4.</remarks>
    Task<ElectrumXBlockHeaderChunk> GetBlockHeaderChunkAsync(long chunkIndex, CancellationToken cancellationToken = default);

    /// <summary>Returns a legacy coin-specific deserialized block header.</summary>
    /// <param name="height">Zero-based block height.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The original JSON value describing the block header.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.block.get_header</c>, available before protocol version 1.4.</remarks>
    Task<ElectrumXJsonValueResult> GetLegacyBlockHeaderAsync(long height, CancellationToken cancellationToken = default);

    /// <summary>Returns the Base58 address associated with a legacy UTXO query.</summary>
    /// <param name="transactionHash">Transaction hash in hexadecimal.</param>
    /// <param name="outputIndex">Zero-based output index.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The address, or JSON null when the output has no address.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.utxo.get_address</c>, available only in protocol version 1.0.</remarks>
    Task<ElectrumXUtxoAddress> GetUtxoAddressAsync(
        string transactionHash,
        int outputIndex,
        CancellationToken cancellationToken = default);

    /// <summary>Subscribes to legacy block-height updates and returns the current chain height.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The current chain height. Later updates are raised through <see cref="BlockHeightReceived" />.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.numblocks.subscribe</c>, available only in protocol version 1.0.</remarks>
    Task<ElectrumXBlockHeight> SubscribeToBlockHeightAsync(CancellationToken cancellationToken = default);

    /// <summary>Estimates the transaction fee rate per kilobyte.</summary>
    /// <param name="numberOfBlocks">Number of blocks within which confirmation is targeted.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The estimated fee in whole coin units per kilobyte; the server may return <c>-1</c> when it cannot estimate.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.estimatefee</c>, available since protocol version 1.0. ElectrumX marks this method deprecated since protocol
    ///     version 1.4.2.
    /// </remarks>
    Task<ElectrumXEstimateFee> EstimateFeeAsync(int numberOfBlocks, CancellationToken cancellationToken = default);

    /// <summary>Estimates the transaction fee using an optional daemon-specific estimation mode.</summary>
    /// <param name="numberOfBlocks">Number of blocks within which confirmation is targeted.</param>
    /// <param name="mode">Optional daemon-specific mode, such as <c>unset</c>, <c>economical</c>, or <c>conservative</c>.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The estimated fee in whole coin units per kilobyte; the server may return <c>-1</c> when it cannot estimate.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.estimatefee</c>; <paramref name="mode" /> requires protocol version 1.6.0 and is only documented for BTC/LTC.
    /// </remarks>
    Task<ElectrumXEstimateFee> EstimateFeeAsync(
        int numberOfBlocks,
        string? mode,
        CancellationToken cancellationToken = default);

    /// <summary>Subscribes to block-header updates and returns the current chain tip.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>
    ///     The current chain tip. Subsequent tip updates are raised through <see cref="BlockHeaderReceived" />; intermediate headers are not guaranteed
    ///     during rapid updates or reorganizations.
    /// </returns>
    /// <remarks>JSON-RPC method: <c>blockchain.headers.subscribe</c>, available since protocol version 1.0.</remarks>
    Task<ElectrumXBlockHeaderSubscription> SubscribeToHeadersAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns the minimum relay fee accepted by the daemon's memory pool.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The minimum relay fee in whole coin units.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.relayfee</c>, available since protocol version 1.0. ElectrumX marks this method deprecated since protocol version
    ///     1.4.2.
    /// </remarks>
    Task<ElectrumXRelayFee> GetRelayFeeAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns the confirmed and unconfirmed balances associated with a script hash.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order (least-significant byte first).</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Confirmed and unconfirmed balances in the coin's smallest units.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.scripthash.get_balance</c>, available since protocol version 1.1.</remarks>
    Task<ElectrumXBalance> GetBalanceAsync(string scriptHash, CancellationToken cancellationToken = default);

    /// <summary>Returns the balance for script-hash outputs selected by a CashToken filter.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order.</param>
    /// <param name="tokenFilter">Optional filter: <c>include_tokens</c>, <c>exclude_tokens</c>, or <c>tokens_only</c>.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Confirmed and unconfirmed balances in the coin's smallest units.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.scripthash.get_balance</c>, available since protocol version 1.1; CashToken filtering requires protocol version
    ///     1.5.0.
    /// </remarks>
    Task<ElectrumXBalance> GetBalanceAsync(string scriptHash, string? tokenFilter, CancellationToken cancellationToken = default);

    /// <summary>Returns confirmed transactions and relevant unconfirmed transactions for a script hash.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order (least-significant byte first).</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Confirmed history in blockchain order followed by the script hash's mempool transactions.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.scripthash.get_history</c>, available since protocol version 1.1.</remarks>
    Task<ElectrumXHistory> GetHistoryAsync(string scriptHash, CancellationToken cancellationToken = default);

    /// <summary>Returns a paginated range of confirmed transactions and, when requested, mempool transactions for a script hash.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order.</param>
    /// <param name="fromHeight">First block height to include; zero-based and inclusive.</param>
    /// <param name="toHeight">Last block height to exclude, or <c>-1</c> to include the chain tip and mempool.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Transactions in blockchain order followed by the canonical mempool ordering.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.scripthash.get_history</c>, available since protocol version 1.1; pagination is available from protocol version
    ///     1.5.1.
    /// </remarks>
    Task<ElectrumXHistory> GetHistoryAsync(
        string scriptHash,
        long fromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXHistoryQuery.ToHeight,
        CancellationToken cancellationToken = default);

    /// <summary>Returns unconfirmed transactions associated with a script hash.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order (least-significant byte first).</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Mempool transactions; their order is unspecified. Heights are <c>0</c> when all inputs are confirmed and <c>-1</c> otherwise.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.scripthash.get_mempool</c>, available since protocol version 1.1.</remarks>
    Task<ElectrumXMempool> GetMempoolAsync(string scriptHash, CancellationToken cancellationToken = default);

    /// <summary>Returns the unspent outputs associated with a script hash.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order (least-significant byte first).</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>
    ///     Unspent outputs in blockchain order, including unspent mempool outputs. Mempool entries are appended in unspecified order; outputs spent in the
    ///     mempool are omitted.
    /// </returns>
    /// <remarks>JSON-RPC method: <c>blockchain.scripthash.listunspent</c>, available since protocol version 1.1.</remarks>
    Task<ElectrumXUnspentOutputs> ListUnspentAsync(string scriptHash, CancellationToken cancellationToken = default);

    /// <summary>Returns script-hash unspent outputs selected by a CashToken filter.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order.</param>
    /// <param name="tokenFilter">Optional filter: <c>include_tokens</c>, <c>exclude_tokens</c>, or <c>tokens_only</c>.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Unspent outputs in blockchain order, including optional CashToken data.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.scripthash.listunspent</c>, available since protocol version 1.1; CashToken filtering requires protocol version
    ///     1.5.0.
    /// </remarks>
    Task<ElectrumXUnspentOutputs> ListUnspentAsync(string scriptHash, string? tokenFilter, CancellationToken cancellationToken = default);

    /// <summary>Returns the raw block header at the specified height as hexadecimal.</summary>
    /// <param name="height">Zero-based main-chain block height; must be non-negative.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The block header encoded as hexadecimal.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.block.header</c>, available since protocol version 1.3; checkpoint proofs are disabled by this overload.</remarks>
    Task<ElectrumXBlockHeader> GetBlockHeaderAsync(long height, CancellationToken cancellationToken = default);

    /// <summary>Returns a block header and its Merkle proof up to the specified checkpoint.</summary>
    /// <param name="height">Zero-based main-chain block height; must be non-negative and no greater than <paramref name="checkpointHeight" />.</param>
    /// <param name="checkpointHeight">Checkpoint height; must be greater than zero.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The requested header, Merkle branch, and checkpoint root.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.block.header</c>, available since protocol version 1.3; checkpoint proofs are available since protocol version 1.4.</remarks>
    Task<ElectrumXBlockHeaderProof> GetBlockHeaderWithProofAsync(long height, long checkpointHeight, CancellationToken cancellationToken = default);

    /// <summary>Subscribes to status changes for a script hash and returns its current status.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order (least-significant byte first).</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The current script-hash status, or <see langword="null" /> when there are no transactions.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.scripthash.subscribe</c>, available since protocol version 1.1. Changes are raised through
    ///     <see cref="ScriptHashStatusChanged" />.
    /// </remarks>
    Task<ElectrumXScriptHashSubscription> SubscribeToScriptHashAsync(string scriptHash, CancellationToken cancellationToken = default);

    /// <summary>Unsubscribes from status-change notifications for a script hash.</summary>
    /// <param name="scriptHash">32-byte script hash encoded as exactly 64 hexadecimal characters, in Electrum byte order (least-significant byte first).</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>
    ///     <see langword="true" /> if the server reports the subscription was removed; otherwise <see langword="false" />. A server may have dropped a
    ///     subscription earlier.
    /// </returns>
    /// <remarks>JSON-RPC method: <c>blockchain.scripthash.unsubscribe</c>, available since protocol version 1.4.2.</remarks>
    Task<ElectrumXScriptHashUnsubscription> UnsubscribeFromScriptHashAsync(string scriptHash, CancellationToken cancellationToken = default);

    /// <summary>Returns a raw transaction by its transaction hash.</summary>
    /// <param name="transactionHash">32-byte transaction hash encoded as exactly 64 hexadecimal characters.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The serialized transaction as hexadecimal.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.get</c>, available since protocol version 1.0, with <c>verbose=false</c>.</remarks>
    Task<ElectrumXRawTransaction> GetRawTransactionAsync(string transactionHash, CancellationToken cancellationToken = default);

    /// <summary>Returns a coin-specific verbose representation of a transaction.</summary>
    /// <param name="transactionHash">32-byte transaction hash encoded as exactly 64 hexadecimal characters.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The coin daemon's verbose transaction representation; its fields depend on the supported coin.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.transaction.get</c>, available since protocol version 1.0; the <c>verbose</c> argument was added in protocol
    ///     version 1.2.
    /// </remarks>
    Task<ElectrumXVerboseTransaction> GetVerboseTransactionAsync(string transactionHash, CancellationToken cancellationToken = default);

    /// <summary>Broadcasts a raw transaction to the network.</summary>
    /// <param name="rawTransaction">Non-empty serialized transaction encoded as an even number of hexadecimal characters.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The transaction hash reported by the server. Acceptance and propagation depend on the server and network.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.transaction.broadcast</c>, available since protocol version 1.0; errors are returned as JSON-RPC errors since
    ///     protocol version 1.1.
    /// </remarks>
    Task<ElectrumXBroadcastTransaction> BroadcastTransactionAsync(string rawTransaction, CancellationToken cancellationToken = default);

    /// <summary>Broadcasts a topologically sorted transaction package (BTC servers that advertise support).</summary>
    /// <param name="rawTransactions">Package transactions, with the child transaction last.</param>
    /// <param name="verbose">Whether to request the daemon-specific verbose response.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The raw JSON result; its shape depends on the requested verbosity and backing daemon.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.broadcast_package</c>, available since protocol version 1.6.0.</remarks>
    Task<ElectrumXJsonValueResult> BroadcastTransactionPackageAsync(
        IReadOnlyList<string> rawTransactions,
        bool verbose = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBroadcastPackageQuery.Verbose,
        CancellationToken cancellationToken = default);

    /// <summary>Returns the Merkle branch proving a confirmed transaction's inclusion in a block.</summary>
    /// <param name="transactionHash">32-byte transaction hash encoded as exactly 64 hexadecimal characters.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The transaction's block height, zero-based position, and Merkle branch.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.transaction.get_merkle</c>, available since protocol version 1.0; the height argument became optional in protocol
    ///     version 1.4.5.
    /// </remarks>
    Task<ElectrumXTransactionMerkle> GetTransactionMerkleAsync(string transactionHash, CancellationToken cancellationToken = default);

    /// <summary>Returns the Merkle branch proving a confirmed transaction's inclusion in a block.</summary>
    /// <param name="transactionHash">32-byte transaction hash encoded as exactly 64 hexadecimal characters.</param>
    /// <param name="height">Optional zero-based height of the block containing the transaction.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The transaction's block height, zero-based position, and Merkle branch.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.transaction.get_merkle</c>, available since protocol version 1.0; the height may be omitted from protocol version
    ///     1.4.5.
    /// </remarks>
    Task<ElectrumXTransactionMerkle> GetTransactionMerkleAsync(string transactionHash, long height, CancellationToken cancellationToken = default);

    /// <summary>Returns a standardized TSC Merkle proof containing the transaction hash.</summary>
    /// <param name="transactionHash">32-byte transaction hash encoded as exactly 64 hexadecimal characters.</param>
    /// <param name="height">Zero-based height of the block containing the transaction; must be non-negative.</param>
    /// <param name="targetType">Proof target: block hash, block header, or Merkle root. Defaults to <see cref="TscMerkleTargetType.BlockHash" />.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A TSC-format branch proof whose transaction field contains the transaction ID.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.transaction.get_tsc_merkle</c>. This method is not specified by the Electrum Cash protocol; availability depends on
    ///     the server implementation. ElectrumX supports branch proofs, not composite proofs.
    /// </remarks>
    Task<ElectrumXTscMerkleWithTransactionHash> GetTscMerkleWithTransactionHashAsync(
        string transactionHash,
        long height,
        TscMerkleTargetType targetType =
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXTscMerkleWithTransactionHashQuery.TargetType,
        CancellationToken cancellationToken = default);

    /// <summary>Returns a standardized TSC Merkle proof containing the full transaction as hexadecimal.</summary>
    /// <param name="transactionHash">32-byte transaction hash encoded as exactly 64 hexadecimal characters.</param>
    /// <param name="height">Zero-based height of the block containing the transaction; must be non-negative.</param>
    /// <param name="targetType">Proof target: block hash, block header, or Merkle root. Defaults to <see cref="TscMerkleTargetType.BlockHash" />.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A TSC-format branch proof whose transaction field contains the full transaction as hexadecimal.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>blockchain.transaction.get_tsc_merkle</c>. This method is not specified by the Electrum Cash protocol; availability depends on
    ///     the server implementation. ElectrumX supports branch proofs, not composite proofs.
    /// </remarks>
    Task<ElectrumXTscMerkleWithTransactionHex> GetTscMerkleWithTransactionHexAsync(
        string transactionHash,
        long height,
        TscMerkleTargetType targetType =
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXTscMerkleWithTransactionHexQuery.TargetType,
        CancellationToken cancellationToken = default);

    /// <summary>Returns the transaction hash at a specified block position, optionally with its Merkle branch.</summary>
    /// <param name="height">Zero-based main-chain block height; must be non-negative.</param>
    /// <param name="transactionPosition">Zero-based transaction index within the block; must be non-negative.</param>
    /// <param name="merkle">Whether to include the transaction's Merkle branch.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The transaction hash and, when requested, its Merkle branch.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.id_from_pos</c>, available since protocol version 1.4.</remarks>
    Task<ElectrumXIdFromPosition> GetTransactionIdFromPositionAsync(
        long height,
        int transactionPosition,
        bool merkle = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXTransactionIdFromPositionQuery.Merkle,
        CancellationToken cancellationToken = default);

    /// <summary>Returns the balance for an address, optionally filtered by CashToken membership.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="tokenFilter">Optional CashToken filter, supported from protocol version 1.5.0.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Confirmed and unconfirmed balances in the coin's smallest units.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.get_balance</c>, available since protocol version 1.4.3.</remarks>
    Task<ElectrumXBalance> GetAddressBalanceAsync(
        string address,
        string? tokenFilter = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressBalanceQuery.TokenFilter,
        CancellationToken cancellationToken = default);

    /// <summary>Returns an address's confirmed and unconfirmed transaction history.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="fromHeight">Inclusive starting height; pagination requires protocol version 1.5.1.</param>
    /// <param name="toHeight">Exclusive ending height, or -1 to include mempool transactions.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>History entries returned by the server.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.get_history</c>, available since protocol version 1.4.3.</remarks>
    Task<ElectrumXHistory> GetAddressHistoryAsync(
        string address,
        long fromHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.FromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressHistoryQuery.ToHeight,
        CancellationToken cancellationToken = default);

    /// <summary>Returns an address's unconfirmed transactions.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Mempool entries returned by the server.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.get_mempool</c>, available since protocol version 1.4.3.</remarks>
    Task<ElectrumXMempool> GetAddressMempoolAsync(string address, CancellationToken cancellationToken = default);

    /// <summary>Returns an address's unspent outputs, optionally filtered by CashToken membership.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="tokenFilter">Optional CashToken filter, supported from protocol version 1.5.0.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Unspent outputs returned by the server.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.listunspent</c>, available since protocol version 1.4.3.</remarks>
    Task<ElectrumXUnspentOutputs> ListAddressUnspentAsync(
        string address,
        string? tokenFilter = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXAddressListUnspentQuery.TokenFilter,
        CancellationToken cancellationToken = default);

    /// <summary>Returns the script hash for an address.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The server-computed script hash.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.get_scripthash</c>, available since protocol version 1.4.3.</remarks>
    Task<ElectrumXAddressScriptHash> GetAddressScriptHashAsync(string address, CancellationToken cancellationToken = default);

    /// <summary>Returns the first on-chain or mempool use of an address.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The first-use result, or <see langword="null" /> when unused.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.get_first_use</c>, available since protocol version 1.5.2.</remarks>
    Task<ElectrumXFirstUse?> GetAddressFirstUseAsync(string address, CancellationToken cancellationToken = default);

    /// <summary>Returns the current address status without subscribing.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The status string, or <see langword="null" /> when unused.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.get_status</c>, available since protocol version 1.6; supported only by Fulcrum 2.1.1 or later.</remarks>
    Task<ElectrumXAddressStatus> GetAddressStatusAsync(string address, CancellationToken cancellationToken = default);

    /// <summary>Subscribes to status changes for an address.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The current address status, or <see langword="null" /> when the address has no status.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.subscribe</c>, available since protocol version 1.4.3.</remarks>
    Task<ElectrumXAddressSubscription> SubscribeToAddressAsync(string address, CancellationToken cancellationToken = default);

    /// <summary>Unsubscribes from status changes for an address.</summary>
    /// <param name="address">Cash Address or supported legacy address.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A JSON boolean indicating whether the server removed the subscription.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.address.unsubscribe</c>, available since protocol version 1.4.3.</remarks>
    Task<ElectrumXAddressUnsubscription> UnsubscribeFromAddressAsync(string address, CancellationToken cancellationToken = default);

    /// <summary>Returns information about an unspent transaction output.</summary>
    /// <param name="transactionHash">Transaction hash identifying the output.</param>
    /// <param name="outputIndex">Zero-based transaction output index.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The UTXO information, or <see langword="null" /> if it is unavailable.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.utxo.get_info</c>, available since protocol version 1.4.4.</remarks>
    Task<ElectrumXUtxoInfo?> GetUnspentOutputInfoAsync(
        string transactionHash,
        int outputIndex,
        CancellationToken cancellationToken = default);

    /// <summary>Returns a transaction's confirmation height, zero for mempool transactions, or null if unknown.</summary>
    /// <param name="transactionHash">Transaction hash.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The result height, or <see langword="null" /> if unknown.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.get_height</c>, available since protocol version 1.4.5.</remarks>
    Task<ElectrumXTransactionHeightResult> GetTransactionHeightAsync(string transactionHash, CancellationToken cancellationToken = default);

    /// <summary>Subscribes to confirmation-height changes for a transaction.</summary>
    /// <param name="transactionHash">Transaction hash.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The transaction's current height, or <see langword="null" /> if unknown.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.subscribe</c>, available since protocol version 1.4.5.</remarks>
    Task<ElectrumXTransactionSubscription> SubscribeToTransactionAsync(string transactionHash, CancellationToken cancellationToken = default);

    /// <summary>Unsubscribes from confirmation-height changes for a transaction.</summary>
    /// <param name="transactionHash">Transaction hash.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A JSON boolean indicating whether the server removed the subscription.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.unsubscribe</c>, available since protocol version 1.4.5.</remarks>
    Task<ElectrumXTransactionUnsubscription> UnsubscribeFromTransactionAsync(
        string transactionHash,
        CancellationToken cancellationToken = default);

    /// <summary>Returns a double-spend proof by transaction hash or proof identifier.</summary>
    /// <param name="hash">Transaction hash or dsproof identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The proof result, whose value is <see langword="null" /> if no proof exists.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.dsproof.get</c>, available since protocol version 1.4.5.</remarks>
    Task<ElectrumXDoubleSpendProof?> GetDoubleSpendProofAsync(
        string hash,
        CancellationToken cancellationToken = default);

    /// <summary>Lists transaction hashes currently associated with double-spend proofs.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The transaction hashes currently associated with proofs.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.dsproof.list</c>, available since protocol version 1.4.5.</remarks>
    Task<ElectrumXStringList> GetDoubleSpendProofTransactionsAsync(CancellationToken cancellationToken = default);

    /// <summary>Subscribes to double-spend-proof updates for a transaction.</summary>
    /// <param name="transactionHash">Transaction hash.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The current proof, or <see langword="null" /> when none exists.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.dsproof.subscribe</c>, available since protocol version 1.4.5.</remarks>
    Task<ElectrumXDoubleSpendProof?> SubscribeToDoubleSpendProofAsync(
        string transactionHash,
        CancellationToken cancellationToken = default);

    /// <summary>Unsubscribes from double-spend-proof updates for a transaction.</summary>
    /// <param name="transactionHash">Transaction hash.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A JSON boolean indicating whether the server removed the subscription.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.dsproof.unsubscribe</c>, available since protocol version 1.4.5.</remarks>
    Task<ElectrumXDoubleSpendProofUnsubscription> UnsubscribeFromDoubleSpendProofAsync(
        string transactionHash,
        CancellationToken cancellationToken = default);

    /// <summary>Returns the block hash containing a confirmed transaction.</summary>
    /// <param name="transactionHash">Transaction hash.</param>
    /// <param name="includeHeader">Whether to include the raw block header in the response.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The block hash, height, and optional header.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.transaction.get_confirmed_blockhash</c>, available since protocol version 1.5.2.</remarks>
    Task<ElectrumXConfirmedTransactionBlockHash> GetConfirmedTransactionBlockHashAsync(
        string transactionHash,
        bool includeHeader =
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXConfirmedTransactionBlockHashQuery.IncludeHeader,
        CancellationToken cancellationToken = default);

    /// <summary>Returns a block header by its hash.</summary>
    /// <param name="blockHash">64-character block hash.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The header's height and hexadecimal encoding.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.header.get</c>, available since protocol version 1.5.2.</remarks>
    Task<ElectrumXHeaderInfo> GetHeaderAsync(string blockHash, CancellationToken cancellationToken = default);

    /// <summary>Returns a block header by its height.</summary>
    /// <param name="height">Block height.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The header's height and hexadecimal encoding.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.header.get</c>, available since protocol version 1.5.2.</remarks>
    Task<ElectrumXHeaderInfo> GetHeaderAsync(long height, CancellationToken cancellationToken = default);

    /// <summary>Returns the current chain tip's height and header.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The tip height and raw header.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.headers.get_tip</c>, available since protocol version 1.5.0.</remarks>
    Task<ElectrumXHeaderInfo> GetHeaderTipAsync(CancellationToken cancellationToken = default);

    /// <summary>Unsubscribes from block-header updates.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>A JSON boolean indicating whether the server removed the subscription.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.headers.unsubscribe</c>, available since protocol version 1.5.0.</remarks>
    Task<ElectrumXHeadersUnsubscription> UnsubscribeFromHeadersAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns the current status of a script hash without subscribing.</summary>
    /// <param name="scriptHash">Script hash in Electrum byte order.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The status string, or <see langword="null" /> when unused.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.scripthash.get_status</c>, available since protocol version 1.6; supported only by Fulcrum 2.1.1 or later.</remarks>
    Task<ElectrumXScriptHashStatus> GetScriptHashStatusAsync(string scriptHash, CancellationToken cancellationToken = default);

    /// <summary>Returns the first on-chain or mempool use of a script hash.</summary>
    /// <param name="scriptHash">Script hash in Electrum byte order.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The first-use result, or <see langword="null" /> when unused.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.scripthash.get_first_use</c>, available since protocol version 1.5.2.</remarks>
    Task<ElectrumXFirstUse?> GetScriptHashFirstUseAsync(string scriptHash, CancellationToken cancellationToken = default);

    /// <summary>Returns the confirmed history for an RPA prefix.</summary>
    /// <param name="rpaPrefix">Hexadecimal RPA prefix supported by the server's RPA index.</param>
    /// <param name="fromHeight">Inclusive first height to scan.</param>
    /// <param name="toHeight">Exclusive end height, or -1 to scan up to the server's limit.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Matching confirmed transactions.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.rpa.get_history</c>, available since protocol version 1.5.3.</remarks>
    Task<ElectrumXHistoryList> GetRpaHistoryAsync(
        string rpaPrefix,
        long fromHeight,
        long toHeight = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXRpaHistoryQuery.ToHeight,
        CancellationToken cancellationToken = default);

    /// <summary>Returns mempool transactions matching an RPA prefix.</summary>
    /// <param name="rpaPrefix">Hexadecimal RPA prefix supported by the server's RPA index.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Matching mempool transactions.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.rpa.get_mempool</c>, available since protocol version 1.5.3.</remarks>
    Task<ElectrumXMempoolEntryList> GetRpaMempoolAsync(string rpaPrefix, CancellationToken cancellationToken = default);

    /// <summary>Returns confirmed RPA transactions using the legacy reusable-address API.</summary>
    /// <param name="fromHeight">Inclusive first height to scan.</param>
    /// <param name="count">Number of blocks to scan.</param>
    /// <param name="rpaPrefix">Hexadecimal RPA prefix supported by the server's RPA index.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Matching confirmed transactions.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.reusable.get_history</c>, available since protocol version 1.5.3.</remarks>
    Task<ElectrumXHistoryList> GetReusableHistoryAsync(
        long fromHeight,
        int count,
        string rpaPrefix,
        CancellationToken cancellationToken = default);

    /// <summary>Returns mempool RPA transactions using the legacy reusable-address API.</summary>
    /// <param name="rpaPrefix">Hexadecimal RPA prefix supported by the server's RPA index.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Matching mempool transactions.</returns>
    /// <remarks>JSON-RPC method: <c>blockchain.reusable.get_mempool</c>, available since protocol version 1.5.3.</remarks>
    Task<ElectrumXMempoolEntryList> GetReusableMempoolAsync(string rpaPrefix,
        CancellationToken cancellationToken = default);
}
