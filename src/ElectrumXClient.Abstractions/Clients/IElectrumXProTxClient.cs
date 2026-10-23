using NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.Abstractions.Clients;

/// <summary>Provides operations for the ProTx-related subset of ElectrumX.</summary>
public interface IElectrumXProTxClient : IAsyncDisposable
{
    /// <summary>Server software and negotiated protocol version.</summary>
    /// <remarks>Negotiated using JSON-RPC method <c>server.version</c>.</remarks>
    ElectrumXServerVersion ServerVersion { get; }

    /// <summary>Returns deterministic masternode-list changes and proof data between two block heights.</summary>
    /// <param name="baseHeight">Starting block height; must be at least <c>1</c>.</param>
    /// <param name="height">Ending block height; must be greater than or equal to <paramref name="baseHeight" />.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The deterministic masternode-list diff and associated proof data.</returns>
    /// <remarks>JSON-RPC method: <c>protx.diff</c>; no Electrum Cash protocol version is specified. Available only on servers supporting Dash DIP3.</remarks>
    Task<ElectrumXProTxDiff> GetDiffAsync(long baseHeight, long height, CancellationToken cancellationToken = default);

    /// <summary>Returns detailed information about a deterministic masternode.</summary>
    /// <param name="proTxHash">32-byte ProRegTx hash encoded as exactly 64 hexadecimal characters.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Detailed deterministic masternode data.</returns>
    /// <remarks>JSON-RPC method: <c>protx.info</c>; no Electrum Cash protocol version is specified. Available only on servers supporting Dash DIP3.</remarks>
    Task<ElectrumXProTxInfo> GetInfoAsync(string proTxHash, CancellationToken cancellationToken = default);
}
