using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.Abstractions.Clients;

/// <summary>Provides operations for the mempool-related subset of ElectrumX.</summary>
public interface IElectrumXMempoolClient : IAsyncDisposable
{
    /// <summary>Server software and negotiated protocol version.</summary>
    /// <remarks>Negotiated using JSON-RPC method <c>server.version</c>.</remarks>
    ElectrumXServerVersion ServerVersion { get; }

    /// <summary>Returns a size-weighted histogram of fee rates paid by transactions in the memory pool.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>Fee-rate and cumulative virtual-size buckets. Bucket boundaries and sizes are server-defined.</returns>
    /// <remarks>
    ///     JSON-RPC method: <c>mempool.get_fee_histogram</c>, available since protocol version 1.2. ElectrumX marks this method deprecated since protocol
    ///     version 1.4.2.
    /// </remarks>
    Task<ElectrumXFeeHistogram> GetFeeHistogramAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns daemon-specific dynamic mempool relay and policy information.</summary>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The available mempool policy values exposed by the server's backing daemon.</returns>
    /// <remarks>JSON-RPC method: <c>mempool.get_info</c>, available since protocol version 1.6.0.</remarks>
    Task<ElectrumXMempoolInfo> GetInfoAsync(CancellationToken cancellationToken = default);
}
