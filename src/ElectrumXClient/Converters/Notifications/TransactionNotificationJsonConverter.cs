using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Notifications;

public sealed class TransactionNotificationJsonConverter : JsonConverter<ElectrumXTransactionNotification>
{
    public override ElectrumXTransactionNotification Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        const string errorMessage = "Transaction notification parameters must contain a string transaction hash and an integer height or null.";
        if (reader.TokenType != JsonTokenType.StartArray || !reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException(errorMessage);
        }

        string transactionHash = reader.GetString() ?? throw new JsonException(errorMessage);
        if (!reader.Read())
        {
            throw new JsonException(errorMessage);
        }

        long? height = reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetInt64(out long parsedHeight) => parsedHeight,
            JsonTokenType.Null => null,
            _ => throw new JsonException(errorMessage)
        };

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException(errorMessage);
        }

        return new ElectrumXTransactionNotification(transactionHash, height);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXTransactionNotification value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        if (value.Height.HasValue)
        {
            writer.WriteNumberValue(value.Height.Value);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteEndArray();
    }
}
