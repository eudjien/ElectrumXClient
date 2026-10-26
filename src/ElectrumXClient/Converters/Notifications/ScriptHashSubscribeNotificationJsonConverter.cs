using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Notifications;

/// <summary>Converts script-hash subscription notification parameters.</summary>
public sealed class ScriptHashSubscribeNotificationJsonConverter
    : JsonConverter<ElectrumXScriptHashNotification>
{
    public override bool HandleNull => true;

    public override ElectrumXScriptHashNotification Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Script-hash notification parameters must be an array.");
        }

        reader.Read();
        string scriptHash = reader.TokenType == JsonTokenType.String
            ? reader.GetString() ?? throw new JsonException("The script hash notification parameter was null.")
            : throw new JsonException("The script hash notification parameter must be a string.");
        reader.Read();
        string? status;
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                status = reader.GetString();
                break;
            case JsonTokenType.Null:
                status = null;
                break;
            default:
                throw new JsonException("The script-hash status notification parameter must be a string or null.");
        }

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Script-hash notification parameters must contain exactly two items.");
        }

        return new ElectrumXScriptHashNotification(scriptHash, status);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXScriptHashNotification value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.ScriptHash);
        if (value.Status is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value.Status);
        }

        writer.WriteEndArray();
    }
}
