using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

public sealed class JsonRpcRequestConverterJsonFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(JsonRpcRequest<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var dataType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(JsonRpcRequestConverter<>).MakeGenericType(dataType);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }

    internal static void WriteJsonRpcId(Utf8JsonWriter writer, JsonRpcId id)
    {
        switch (id.Kind)
        {
            case JsonRpcIdKind.String:
                writer.WriteString(ElectrumXJsonPropertyNames.JsonRpcRequest.Id, id.String);
                break;
            case JsonRpcIdKind.Number:
                writer.WriteNumber(ElectrumXJsonPropertyNames.JsonRpcRequest.Id, id.Number.GetValueOrDefault());
                break;
            case JsonRpcIdKind.Null:
                writer.WriteNull(ElectrumXJsonPropertyNames.JsonRpcRequest.Id);
                break;
            default:
                throw new JsonException("A JSON-RPC request ID must be an integer, string, or null.");
        }
    }

    private sealed class JsonRpcRequestConverter<TQuery> : JsonConverter<JsonRpcRequest<TQuery>>
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

            WriteJsonRpcId(writer, value.Id);

            writer.WriteString(ElectrumXJsonPropertyNames.JsonRpcRequest.Method, value.Method);
            writer.WritePropertyName(ElectrumXJsonPropertyNames.JsonRpcRequest.Parameters);

            var queryConverter = (JsonConverter<TQuery>)options.GetConverter(typeof(TQuery));
            queryConverter.Write(writer, value.Query, options);

            writer.WriteEndObject();
        }
    }
}
