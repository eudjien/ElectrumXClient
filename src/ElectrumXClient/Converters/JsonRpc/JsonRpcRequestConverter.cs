using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

internal sealed class JsonRpcRequestConverter<TQuery> : JsonConverter<JsonRpcRequest<TQuery>>
    where TQuery : ElectrumXQuery
{
    public override bool HandleNull => true;

    public override JsonRpcRequest<TQuery> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Deserializing JSON-RPC requests is not supported.");
    }

    public override void Write(Utf8JsonWriter writer, JsonRpcRequest<TQuery> value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("Expected a JSON-RPC request.");
        }

        writer.WriteStartObject();

        writer.WriteString(ElectrumXJsonPropertyNames.JsonRpcVersion, value.JsonRpcVersion);

        switch (value.Id.Kind)
        {
            case JsonRpcIdKind.String:
                writer.WriteString(ElectrumXJsonPropertyNames.JsonRpcRequest.Id, value.Id.String);
                break;
            case JsonRpcIdKind.Number:
                writer.WriteNumber(ElectrumXJsonPropertyNames.JsonRpcRequest.Id, value.Id.Number.GetValueOrDefault());
                break;
            case JsonRpcIdKind.Null:
                writer.WriteNull(ElectrumXJsonPropertyNames.JsonRpcRequest.Id);
                break;
            default:
                throw new JsonException("A JSON-RPC request ID must be an integer, string, or null.");
        }

        writer.WriteString(ElectrumXJsonPropertyNames.JsonRpcRequest.Method, value.Method);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.JsonRpcRequest.Parameters);

        var queryConverter = (JsonConverter<TQuery>)options.GetConverter(typeof(TQuery));
        queryConverter.Write(writer, value.Query, options);

        writer.WriteEndObject();
    }
}
