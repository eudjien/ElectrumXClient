using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

public sealed class JsonRpcResponseJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(JsonRpcResponse<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var dataType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(JsonRpcResponseJsonConverter<>).MakeGenericType(dataType);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }

    private sealed class JsonRpcResponseJsonConverter<TResult> : JsonConverter<JsonRpcResponse<TResult>> where TResult : ElectrumXResult
    {
        public override bool HandleNull => true;

        public override JsonRpcResponse<TResult> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected a JSON-RPC response object.");
            }

            string? version = null;
            JsonRpcId? id = null;
            TResult? data = null;
            JsonRpcError? error = null;
            bool hasId = false;
            bool hasResult = false;
            bool hasError = false;
            var errorConverter = (JsonConverter<JsonRpcError>)options.GetConverter(typeof(JsonRpcError));
            var dataConverter = (JsonConverter<TResult>)options.GetConverter(typeof(TResult));

            while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected a JSON-RPC response property.");
                }

                string? propertyName = reader.GetString();
                if (!reader.Read())
                {
                    throw new JsonException("The JSON-RPC response property value is missing.");
                }

                switch (propertyName)
                {
                    case ElectrumXJsonPropertyNames.JsonRpcVersion:
                        version = reader.TokenType == JsonTokenType.String
                            ? reader.GetString()
                            : throw new JsonException("The JSON-RPC response version must be a string.");
                        break;
                    case ElectrumXJsonPropertyNames.JsonRpcResponse.Id:
                        hasId = true;
                        id = reader.TokenType switch
                        {
                            JsonTokenType.Number when reader.TryGetInt64(out long number) => JsonRpcId.FromNumber(number),
                            JsonTokenType.String when reader.GetString() is {} text => JsonRpcId.FromString(text),
                            JsonTokenType.Null => JsonRpcId.FromNull(),
                            _ => throw new JsonException("A JSON-RPC response ID must be an integer, string, or null.")
                        };
                        break;
                    case ElectrumXJsonPropertyNames.JsonRpcResponse.Result:
                        hasResult = true;
                        data = reader.TokenType == JsonTokenType.Null && !dataConverter.HandleNull
                            ? null
                            : dataConverter.Read(ref reader, typeof(TResult), options);
                        break;
                    case ElectrumXJsonPropertyNames.JsonRpcResponse.Error:
                        {
                            hasError = true;
                            error = errorConverter.Read(ref reader, typeof(JsonRpcError), options)
                                    ?? throw new JsonException("The JSON-RPC error response did not contain a valid error.");
                            break;
                        }
                    default:
                        reader.Skip();
                        break;
                }
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException("The JSON-RPC response object is incomplete.");
            }

            string jsonRpcVersion = version
                                    ?? throw new JsonException("The JSON-RPC response envelope is incomplete or invalid.");

            var responseId = id
                             ?? throw new JsonException("The JSON-RPC response envelope is incomplete or invalid.");
            if (!hasId || hasResult == hasError)
            {
                throw new JsonException("A JSON-RPC response must contain either result or error.");
            }

            if (hasError)
            {
                return JsonRpcResponse<TResult>.CreateError(
                    responseId,
                    jsonRpcVersion,
                    error ?? throw new JsonException("The JSON-RPC error response is invalid."));
            }

            return JsonRpcResponse<TResult>.Success(responseId, jsonRpcVersion, data);
        }

        public override void Write(
            Utf8JsonWriter writer,
            JsonRpcResponse<TResult> value,
            JsonSerializerOptions options)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            writer.WriteStartObject();
            writer.WriteString(ElectrumXJsonPropertyNames.JsonRpcVersion, value.JsonRpcVersion);
            if (value.Error is not null)
            {
                writer.WritePropertyName(ElectrumXJsonPropertyNames.JsonRpcResponse.Error);
                var errorConverter = (JsonConverter<JsonRpcError>)options.GetConverter(typeof(JsonRpcError));
                errorConverter.Write(writer, value.Error, options);
            }
            else if (value.Result is not null)
            {
                writer.WritePropertyName(ElectrumXJsonPropertyNames.JsonRpcResponse.Result);
                var resultConverter = (JsonConverter<TResult>)options.GetConverter(typeof(TResult));
                resultConverter.Write(writer, value.Result, options);
            }
            else
            {
                writer.WriteNull(ElectrumXJsonPropertyNames.JsonRpcResponse.Result);
            }

            switch (value.Id.Kind)
            {
                case JsonRpcIdKind.Number when value.Id.Number is {} number:
                    writer.WriteNumber(ElectrumXJsonPropertyNames.JsonRpcResponse.Id, number);
                    break;
                case JsonRpcIdKind.String when value.Id.String is {} text:
                    writer.WriteString(ElectrumXJsonPropertyNames.JsonRpcResponse.Id, text);
                    break;
                default:
                    writer.WriteNull(ElectrumXJsonPropertyNames.JsonRpcResponse.Id);
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
