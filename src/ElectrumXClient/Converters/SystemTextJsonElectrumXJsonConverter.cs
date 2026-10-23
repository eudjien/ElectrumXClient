using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters;

/// <summary>ElectrumX JSON adapter backed by System.Text.Json.</summary>
public sealed class SystemTextJsonElectrumXJsonConverter : IElectrumXJsonConverter
{
    private readonly JsonSerializerOptions _options;

    public SystemTextJsonElectrumXJsonConverter()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        _options = options.AddElectrumXConverters();
    }

    /// <inheritdoc />
    public byte[] SerializeRequest<TQuery>(JsonRpcRequest<TQuery> request)
        where TQuery : ElectrumXQuery
    {
        Abstractions.ArgumentGuard.ThrowIfNull(request, nameof(request));
        return JsonSerializer.SerializeToUtf8Bytes(request, request.GetType(), _options);
    }

    /// <inheritdoc />
    public JsonRpcHeader DeserializeHeader(ReadOnlyMemory<byte> json)
    {
        return JsonSerializer.Deserialize<JsonRpcHeader>(json.Span, _options)
               ?? throw new JsonException("The JSON-RPC message header was null.");
    }

    /// <inheritdoc />
    public JsonRpcResponse<TResponse> DeserializeResponse<TResponse>(ReadOnlyMemory<byte> json)
        where TResponse : ElectrumXResult
    {
        return JsonSerializer.Deserialize<JsonRpcResponse<TResponse>>(json.Span, _options)
               ?? throw new JsonException("The JSON-RPC response envelope was null.");
    }

    /// <inheritdoc />
    public JsonRpcNotification<TParams> DeserializeNotification<TParams>(ReadOnlyMemory<byte> json)
        where TParams : ElectrumXNotification
    {
        return JsonSerializer.Deserialize<JsonRpcNotification<TParams>>(json.Span, _options)
               ?? throw new JsonException("The JSON-RPC notification envelope was null.");
    }
}
