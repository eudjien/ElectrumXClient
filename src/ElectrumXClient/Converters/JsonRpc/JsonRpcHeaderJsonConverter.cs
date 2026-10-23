using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

internal sealed class JsonRpcHeaderJsonConverter : JsonConverter<JsonRpcHeader>
{
    public override bool HandleNull => true;

    public override JsonRpcHeader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.None && !reader.Read())
        {
            throw new JsonException("The JSON-RPC message envelope is incomplete or invalid.");
        }

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            reader.Skip();

            return new JsonRpcHeader(
                null,
                false,
                null,
                false,
                null,
                false,
                true,
                false,
                JsonRpcHeader.JsonRpcParametersKind.Missing);
        }

        string? version = null;
        bool hasMethod = false;
        string? method = null;
        bool hasId = false;
        JsonRpcId? id = null;
        var parametersKind = JsonRpcHeader.JsonRpcParametersKind.Missing;
        bool hasResult = false;
        bool hasError = false;
        bool isErrorObject = true;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("The JSON-RPC message envelope is incomplete or invalid.");
            }

            bool isVersion = reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcVersion.AsSpan());
            bool isMethod = reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcResponse.Method.AsSpan());
            bool isId = reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcResponse.Id.AsSpan());
            bool isResult = reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcResponse.Result.AsSpan());
            bool isError = reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcResponse.Error.AsSpan());
            bool isParameters = reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcRequest.Parameters.AsSpan());

            if (!reader.Read())
            {
                throw new JsonException("The JSON-RPC message envelope is incomplete or invalid.");
            }

            if (isVersion)
            {
                version = reader.TokenType == JsonTokenType.String ? reader.GetString() : null;
            }
            else if (isMethod)
            {
                hasMethod = true;
                method = reader.TokenType == JsonTokenType.String ? reader.GetString() : null;
            }
            else if (isId)
            {
                hasId = true;
                id = reader.TokenType switch
                {
                    JsonTokenType.Number when reader.TryGetInt64(out long messageId) => JsonRpcId.FromNumber(messageId),
                    JsonTokenType.String => JsonRpcId.FromString(reader.GetString()
                                                                 ?? throw new JsonException("The JSON-RPC message ID string was null.")),
                    JsonTokenType.Null => JsonRpcId.FromNull(),
                    _ => null
                };
            }
            else if (isParameters)
            {
                parametersKind = reader.TokenType switch
                {
                    JsonTokenType.StartArray => JsonRpcHeader.JsonRpcParametersKind.Array,
                    JsonTokenType.StartObject => JsonRpcHeader.JsonRpcParametersKind.Object,
                    _ => JsonRpcHeader.JsonRpcParametersKind.Other
                };
            }
            else if (isResult)
            {
                hasResult = true;
            }
            else if (isError)
            {
                hasError = true;
                isErrorObject = reader.TokenType == JsonTokenType.StartObject;
            }

            if (reader.TokenType is JsonTokenType.StartArray or JsonTokenType.StartObject)
            {
                reader.Skip();
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The JSON-RPC message envelope is incomplete or invalid.");
        }

        return new JsonRpcHeader(
            id,
            hasId,
            version,
            hasMethod,
            method,
            hasError,
            isErrorObject,
            hasResult,
            parametersKind);
    }

    public override void Write(Utf8JsonWriter writer, JsonRpcHeader value, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Serializing JSON-RPC message headers is not supported.");
    }
}
