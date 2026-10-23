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

    protected IElectrumXRpcClient RpcClient => _rpcClient;

    protected bool OwnsRpcClient => ownsRpcClient;

    public abstract ElectrumXServerVersion ServerVersion { get; }

    public virtual async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        await DisposeCoreAsync().ConfigureAwait(false);
    }

    protected void ThrowIfDisposed()
    {
        if (Interlocked.CompareExchange(ref _disposed, 0, 0) != 0)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    protected void EnsureProtocolMinimumVersion(ElectrumXProtocolVersion minimumVersion, string methodName)
    {
        if (ServerVersion.ProtocolVersion < minimumVersion)
        {
            throw new NotSupportedException(
                $"{methodName} requires protocol version {minimumVersion} or later; the negotiated version is {ServerVersion.ProtocolVersion}.");
        }
    }

    protected async Task<TResult> CallOrThrowAsync<TResult, TQuery>(JsonRpcRequest<TQuery> request, CancellationToken cancellationToken)
        where TResult : ElectrumXResult
        where TQuery : ElectrumXQuery
    {
        return (await RpcClient.CallAsync<TResult, TQuery>(request, cancellationToken).ConfigureAwait(false)).Result
               ?? throw new ElectrumXProtocolException("The ElectrumX result response did not contain data.");
    }

    protected async Task<TResult?> CallAsync<TResult, TQuery>(
        JsonRpcRequest<TQuery> request,
        CancellationToken cancellationToken)
        where TResult : ElectrumXResult
        where TQuery : ElectrumXQuery
    {
        return (await RpcClient.CallAsync<TResult, TQuery>(request, cancellationToken).ConfigureAwait(false)).Result;
    }

    protected virtual ValueTask DisposeCoreAsync()
    {
        return OwnsRpcClient
            ? RpcClient.DisposeAsync()
            : default;
    }
}
