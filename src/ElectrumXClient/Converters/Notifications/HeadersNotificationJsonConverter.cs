using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using NorthGomel.ElectrumXClient.Converters.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Notifications;

public sealed class HeadersNotificationJsonConverter
    : JsonConverter<ElectrumXHeadersNotification>
{
    public override bool HandleNull => true;

    public override ElectrumXHeadersNotification Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Header notification parameters must be an array.");
        }

        if (!reader.Read())
        {
            throw new JsonException("Header notification parameters must contain exactly one item.");
        }

        var valueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
        var value = valueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options)
                    ?? throw new JsonException("The block-header notification payload cannot be null.");

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Header notification parameters must contain exactly one item.");
        }

        (long? height, string? hex, bool hasRawHeight) = ElectrumXHeaderValueReader.ReadFields(value);
        bool isCurrentFormat = hasRawHeight && height.HasValue && hex is not null;
        if (height is < int.MinValue or > int.MaxValue)
        {
            throw new JsonException("The block-header notification height must fit in a 32-bit integer.");
        }

        int notificationHeight = height.HasValue ? (int)height.Value : 0;

        return new ElectrumXHeadersNotification(
            hex ?? string.Empty,
            notificationHeight,
            isCurrentFormat ? null : value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXHeadersNotification? value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        if (value.LegacyHeader is not null)
        {
            var valueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
            valueConverter.Write(writer, value.LegacyHeader, options);
        }
        else
        {
            writer.WriteStartObject();
            writer.WriteNumber(ElectrumXJsonPropertyNames.HeadersNotificationParameters.Height, value.Height);
            writer.WriteString(ElectrumXJsonPropertyNames.HeadersNotificationParameters.Hex, value.Hex);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}
