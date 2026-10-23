using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.JsonRpc;

internal readonly struct Response
{
    internal Response(ReadOnlyMemory<byte> payload)
    {
        Payload = payload;
    }

    internal ReadOnlyMemory<byte> Payload { get; }
}

internal sealed class ElectrumXRpcRequestTracker
{
    private readonly ConcurrentDictionary<JsonRpcId, TaskCompletionSource<Response>> _pendingRequests = new();

    private long _nextRequestId;

    internal ElectrumXRpcRequestRegistration Register(JsonRpcId requestedId)
    {
        var completion = new TaskCompletionSource<Response>(TaskCreationOptions.RunContinuationsAsynchronously);
        var requestId = requestedId.Kind == JsonRpcIdKind.Null
            ? JsonRpcId.FromNumber(Interlocked.Increment(ref _nextRequestId))
            : requestedId;

        var pendingId = requestId;

        while (true)
        {
            if (_pendingRequests.TryAdd(pendingId, completion))
            {
                return new ElectrumXRpcRequestRegistration(pendingId, completion);
            }

            if (requestedId.Kind != JsonRpcIdKind.Null)
            {
                throw new InvalidOperationException($"A JSON-RPC request with ID {pendingId} is already pending.");
            }

            pendingId = JsonRpcId.FromNumber(Interlocked.Increment(ref _nextRequestId));
        }
    }

    internal void Complete(JsonRpcId id, ReadOnlyMemory<byte> response)
    {
        if (_pendingRequests.TryRemove(id, out var completionSource))
        {
            completionSource.TrySetResult(new Response(response.ToArray()));
        }
    }

    internal void CompleteAllWithException(Exception exception)
    {
        ICollection<KeyValuePair<JsonRpcId, TaskCompletionSource<Response>>> pendingRequests = _pendingRequests;

        foreach (var request in _pendingRequests)
        {
            if (pendingRequests.Remove(request))
            {
                request.Value.TrySetException(exception);
            }
        }
    }

    internal void Remove(JsonRpcId id)
    {
        _pendingRequests.TryRemove(id, out _);
    }
}

internal sealed class ElectrumXRpcRequestRegistration
{
    internal ElectrumXRpcRequestRegistration(JsonRpcId id, TaskCompletionSource<Response> completionSource)
    {
        Id = id;
        CompletionSource = completionSource;
    }

    internal JsonRpcId Id { get; }

    internal TaskCompletionSource<Response> CompletionSource { get; }
}
