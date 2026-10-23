using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Notifications;

public sealed class BlockHeightNotificationJsonConverter : JsonConverter<ElectrumXBlockHeightNotification>
{
    public override ElectrumXBlockHeightNotification Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray ||
            !reader.Read() ||
            reader.TokenType != JsonTokenType.Number ||
            !reader.TryGetInt64(out long height))
        {
            throw new JsonException("Block-height notification parameters must contain one integer.");
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Block-height notification parameters must contain exactly one item.");
        }

        return new ElectrumXBlockHeightNotification(height);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXBlockHeightNotification value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Height);
        writer.WriteEndArray();
    }
}
