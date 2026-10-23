using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Notifications;

public sealed class MasterNodeNotificationJsonConverter
    : JsonConverter<ElectrumXMasterNodeNotification>
{
    public override bool HandleNull => true;

    public override ElectrumXMasterNodeNotification Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Masternode notification parameters must be an array.");
        }

        reader.Read();
        string collateral = reader.TokenType == JsonTokenType.String
            ? reader.GetString() ?? throw new JsonException("The masternode collateral notification parameter was null.")
            : throw new JsonException("The masternode collateral notification parameter must be a string.");
        reader.Read();

        string? status = reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Null => null,
            _ => throw new JsonException("The masternode status notification parameter must be a string or null.")
        };

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Masternode notification parameters must contain exactly two items.");
        }

        return new ElectrumXMasterNodeNotification(collateral, status);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXMasterNodeNotification value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Collateral);
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
