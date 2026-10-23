using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Converters.Results;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

public sealed class JsonRpcErrorJsonConverter : JsonConverter<JsonRpcError>
{
    public override bool HandleNull => true;

    public override JsonRpcError Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        int? code = null;
        string? message = null;
        object? data = null;
        var dataConverter = new ElectrumXJsonValueResultJsonConverter();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected an RPC error property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The RPC error property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.RpcErrorResult.Code:
                    code = reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int errorCode)
                        ? errorCode
                        : throw new JsonException("The RPC error code must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.RpcErrorResult.Message:
                    message = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("The RPC error message must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.RpcErrorResult.Data:
                    data = reader.TokenType == JsonTokenType.Null
                        ? null
                        : dataConverter.Read(ref reader, typeof(ElectrumXJsonValue), options);
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The RPC error object is incomplete.");
        }

        return new JsonRpcError(
            code ?? throw new JsonException("The RPC error code is missing."),
            message ?? throw new JsonException("The RPC error message is missing."),
            data);
    }

    public override void Write(Utf8JsonWriter writer, JsonRpcError? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.RpcErrorResult.Code, value.Code);
        writer.WriteString(ElectrumXJsonPropertyNames.RpcErrorResult.Message, value.Message);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.RpcErrorResult.Data);
        switch (value.Data)
        {
            case ElectrumXJsonValue jsonData:
                {
                    var dataConverter = new ElectrumXJsonValueResultJsonConverter();
                    dataConverter.Write(writer, jsonData, options);
                    break;
                }
            case null:
                writer.WriteNullValue();
                break;
            default:
                throw new JsonException("The RPC error data must be a JSON value.");
        }
        writer.WriteEndObject();
    }
}
