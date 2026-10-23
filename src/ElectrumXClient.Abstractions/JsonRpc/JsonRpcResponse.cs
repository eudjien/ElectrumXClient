using NorthGomel.ElectrumXClient.Abstractions.Models;

namespace NorthGomel.ElectrumXClient.Abstractions.JsonRpc;

/// <summary>Represents a JSON-RPC 2.0 response containing a result or an error.</summary>
public sealed class JsonRpcResponse<TResult> where TResult : class
{
    private JsonRpcResponse(JsonRpcId id, string jsonRpcVersion, TResult? result, JsonRpcError? error)
    {
        Id = id;
        JsonRpcVersion = jsonRpcVersion;
        Result = result;
        Error = error;
    }

    /// <summary>The JSON-RPC response identifier.</summary>
    public JsonRpcId Id { get; }

    /// <summary>The JSON-RPC protocol version.</summary>
    public string JsonRpcVersion { get; }

    /// <summary>The result, or <see langword="null" /> when the server returned null or a JSON-RPC error.</summary>
    public TResult? Result { get; }

    /// <summary>The server error, or <see langword="null" /> when the response contains data.</summary>
    public JsonRpcError? Error { get; }

    /// <summary>Creates a successful response containing data.</summary>
    public static JsonRpcResponse<TResult> Success(JsonRpcId id, string jsonRpcVersion, TResult? data)
    {
        ArgumentGuard.ThrowIfNull(id, nameof(id));
        ArgumentGuard.ThrowIfNull(jsonRpcVersion, nameof(jsonRpcVersion));
        return new JsonRpcResponse<TResult>(id, jsonRpcVersion, data, null);
    }

    /// <summary>Creates a failed response containing a JSON-RPC error.</summary>
    public static JsonRpcResponse<TResult> CreateError(JsonRpcId id, string jsonRpcVersion, JsonRpcError error)
    {
        ArgumentGuard.ThrowIfNull(id, nameof(id));
        ArgumentGuard.ThrowIfNull(jsonRpcVersion, nameof(jsonRpcVersion));
        ArgumentGuard.ThrowIfNull(error, nameof(error));

        return new JsonRpcResponse<TResult>(id, jsonRpcVersion, null, error);
    }
}

/// <summary>Represents the error object in a JSON-RPC 2.0 response.</summary>
public sealed class JsonRpcError(int code, string message, object? data)
{
    public int Code { get; } = code;

    public string Message { get; } = message;

    /// <summary>The optional payload, represented by <see cref="ElectrumXJsonValue" /> when deserialized.</summary>
    public object? Data { get; } = data;
}
