using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Notifications;

public sealed class AddressNotificationJsonConverter : JsonConverter<ElectrumXAddressNotification>
{
    public override ElectrumXAddressNotification Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        const string errorMessage = "Address notification parameters must contain a string address and a string or null status.";
        if (reader.TokenType != JsonTokenType.StartArray || !reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException(errorMessage);
        }

        string address = reader.GetString() ?? throw new JsonException(errorMessage);
        if (!reader.Read())
        {
            throw new JsonException(errorMessage);
        }

        string? status = reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Null => null,
            _ => throw new JsonException(errorMessage)
        };

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException(errorMessage);
        }

        return new ElectrumXAddressNotification(address, status);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXAddressNotification value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
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
