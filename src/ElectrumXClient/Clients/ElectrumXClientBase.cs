using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Exceptions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.Clients;

/// <summary>Shared lifecycle behavior for concrete ElectrumX client wrappers.</summary>
public abstract class ElectrumXClientBase(IElectrumXRpcClient rpcClient, bool ownsRpcClient = true) : IAsyncDisposable
{
    private readonly IElectrumXRpcClient _rpcClient = rpcClient ?? throw new ArgumentNullException(nameof(rpcClient));
    private int _disposed;

    /// <summary>Gets the JSON-RPC transport used by this client.</summary>
    protected IElectrumXRpcClient RpcClient => _rpcClient;

    /// <summary>Gets whether this client is responsible for disposing the JSON-RPC transport.</summary>
    protected bool OwnsRpcClient => ownsRpcClient;

    /// <summary>Gets the server software and negotiated protocol version.</summary>
    public abstract ElectrumXServerVersion ServerVersion { get; }

    /// <summary>Disposes this client and, when owned, its JSON-RPC transport.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public virtual async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        await DisposeCoreAsync().ConfigureAwait(false);
    }

    /// <summary>Throws if this client has already been disposed.</summary>
    /// <exception cref="ObjectDisposedException">The client has been disposed.</exception>
    protected void ThrowIfDisposed()
    {
        if (Interlocked.CompareExchange(ref _disposed, 0, 0) != 0)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    /// <summary>Ensures that the negotiated protocol version supports the specified method.</summary>
    /// <param name="minimumVersion">The minimum supported protocol version.</param>
    /// <param name="methodName">The method name used in the exception message.</param>
    /// <exception cref="NotSupportedException">The negotiated protocol version is older than <paramref name="minimumVersion" />.</exception>
    protected void EnsureProtocolMinimumVersion(ElectrumXProtocolVersion minimumVersion, string methodName)
    {
        if (ServerVersion.ProtocolVersion < minimumVersion)
        {
            throw new NotSupportedException(
                $"{methodName} requires protocol version {minimumVersion} or later; the negotiated version is {ServerVersion.ProtocolVersion}.");
        }
    }

    /// <summary>Sends a JSON-RPC request and requires a non-null result.</summary>
    /// <typeparam name="TResult">The expected result model type.</typeparam>
    /// <typeparam name="TQuery">The query model type.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The deserialized result.</returns>
    /// <exception cref="ElectrumXProtocolException">The server response does not contain a result.</exception>
    protected async Task<TResult> CallOrThrowAsync<TResult, TQuery>(JsonRpcRequest<TQuery> request, CancellationToken cancellationToken)
        where TResult : ElectrumXResult
        where TQuery : ElectrumXQuery
    {
        return (await RpcClient.CallAsync<TResult, TQuery>(request, cancellationToken).ConfigureAwait(false)).Result
               ?? throw new ElectrumXProtocolException("The ElectrumX result response did not contain data.");
    }

    /// <summary>Sends a JSON-RPC request and returns its result, which may be null.</summary>
    /// <typeparam name="TResult">The expected result model type.</typeparam>
    /// <typeparam name="TQuery">The query model type.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">Token used to cancel the request.</param>
    /// <returns>The deserialized result, or <see langword="null" /> when the response has no result.</returns>
    protected async Task<TResult?> CallAsync<TResult, TQuery>(
        JsonRpcRequest<TQuery> request,
        CancellationToken cancellationToken)
        where TResult : ElectrumXResult
        where TQuery : ElectrumXQuery
    {
        return (await RpcClient.CallAsync<TResult, TQuery>(request, cancellationToken).ConfigureAwait(false)).Result;
    }

    /// <summary>Releases resources owned by the derived client.</summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    protected virtual ValueTask DisposeCoreAsync()
    {
        return OwnsRpcClient
            ? RpcClient.DisposeAsync()
            : default;
    }
}
