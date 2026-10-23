using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using System;

namespace NorthGomel.ElectrumXClient.Abstractions.JsonRpc;

/// <summary>A JSON-RPC request paired with its strongly typed ElectrumX query.</summary>
public sealed class JsonRpcRequest<TQuery>
    where TQuery : ElectrumXQuery
{
    /// <summary>Creates a request whose identifier is assigned when sent.</summary>
    public JsonRpcRequest(string method, TQuery query)
        : this(JsonRpcId.FromNull(), method, query)
    {
    }

    /// <summary>Creates a request with an explicit JSON-RPC identifier.</summary>
    public JsonRpcRequest(JsonRpcId id, string method, TQuery query)
    {
        if (string.IsNullOrWhiteSpace(method))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(method));
        }

        Id = id ?? throw new ArgumentNullException(nameof(id));
        Method = method;
        Query = query ?? throw new ArgumentNullException(nameof(query));
    }

    /// <summary>The JSON-RPC protocol version.</summary>
    public string JsonRpcVersion => ElectrumXJsonPropertyValues.JsonRpcVersion;

    /// <summary>The request identifier used to match the response.</summary>
    public JsonRpcId Id { get; }

    /// <summary>The ElectrumX method name.</summary>
    public string Method { get; }

    /// <inheritdoc />
    public TQuery Query { get; }

    internal JsonRpcRequest<TQuery> WithId(JsonRpcId id)
    {
        return new JsonRpcRequest<TQuery>(id, Method, Query);
    }
}
