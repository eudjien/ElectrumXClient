using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

internal sealed class JsonRpcNotificationJsonConverter<TParams> : JsonConverter<JsonRpcNotification<TParams>>
    where TParams : ElectrumXNotification
{
    public override bool HandleNull => true;

    public override JsonRpcNotification<TParams> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The JSON-RPC message envelope is incomplete or invalid.");
        }

        string? version = null;
        string? method = null;
        var parametersKind = JsonRpcHeader.JsonRpcParametersKind.Missing;
        TParams? notificationParameters = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
            {
                throw new JsonException("The JSON-RPC message envelope is incomplete or invalid.");
            }

            if (reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcVersion.AsSpan()))
            {
                version = reader.TokenType == JsonTokenType.String ? reader.GetString() : null;
            }
            else if (reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcNotification.Method.AsSpan()))
            {
                method = reader.TokenType == JsonTokenType.String ? reader.GetString() : null;
            }
            else if (reader.ValueTextEquals(ElectrumXJsonPropertyNames.JsonRpcNotification.Parameters.AsSpan()))
            {
                parametersKind = reader.TokenType switch
                {
                    JsonTokenType.StartArray => JsonRpcHeader.JsonRpcParametersKind.Array,
                    JsonTokenType.StartObject => JsonRpcHeader.JsonRpcParametersKind.Object,
                    _ => JsonRpcHeader.JsonRpcParametersKind.Other
                };

                if (parametersKind is JsonRpcHeader.JsonRpcParametersKind.Array or
                    JsonRpcHeader.JsonRpcParametersKind.Object)
                {
                    var parametersConverter = (JsonConverter<TParams>)options.GetConverter(typeof(TParams));
                    notificationParameters = parametersConverter.Read(ref reader, typeof(TParams), options);
                }
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

        string versionValue = version
                              ?? throw new JsonException("The JSON-RPC version was null.");
        string methodValue = method
                             ?? throw new JsonException("The JSON-RPC notification did not contain a string method.");
        if (parametersKind is not (JsonRpcHeader.JsonRpcParametersKind.Array or
            JsonRpcHeader.JsonRpcParametersKind.Object))
        {
            throw new JsonException("The JSON-RPC notification did not contain valid params.");
        }

        return new JsonRpcNotification<TParams>(
            versionValue,
            methodValue,
            notificationParameters
            ?? throw new JsonException("The JSON-RPC notification params were null."));
    }

    public override void Write(
        Utf8JsonWriter writer,
        JsonRpcNotification<TParams> value,
        JsonSerializerOptions options)
    {
        throw new NotSupportedException("Serializing JSON-RPC notifications is not supported.");
    }
}
