using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class BlockHeaderSubscriptionResultJsonConverter : JsonConverter<ElectrumXBlockHeaderSubscription>
{
    public override bool HandleNull => true;

    public override ElectrumXBlockHeaderSubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var valueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
        var value = valueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options)
                    ?? throw new JsonException("The block-header subscription result cannot be null.");
        (long? height, string? hex, bool hasRawHeight) = ElectrumXHeaderValueReader.ReadFields(value);

        bool isCurrentFormat = hasRawHeight && height.HasValue && hex is not null;
        return new ElectrumXBlockHeaderSubscription(
            height ?? 0,
            hex ?? string.Empty,
            isCurrentFormat ? null : value);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXBlockHeaderSubscription value,
        JsonSerializerOptions options)
    {
        if (value.LegacyHeader is not null)
        {
            var valueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
            valueConverter.Write(writer, value.LegacyHeader, options);
            return;
        }

        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.BlockHeaderSubscriptionResult.Height, value.Height);
        writer.WriteString(ElectrumXJsonPropertyNames.BlockHeaderSubscriptionResult.Hex, value.Hex);
        writer.WriteEndObject();
    }
}
