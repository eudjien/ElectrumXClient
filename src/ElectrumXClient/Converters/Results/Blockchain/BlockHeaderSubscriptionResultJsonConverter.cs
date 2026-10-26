using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Globalization;
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
        long? height = null;
        string? hex = null;
        bool hasRawHeight = false;
        if (value is ElectrumXJsonValue.ObjectValue objectValue)
        {
            foreach ((string name, var propertyValue) in objectValue.Properties)
            {
                switch (name)
                {
                    case ElectrumXJsonPropertyNames.BlockHeaderSubscriptionResult.Height:
                        hasRawHeight = true;
                        goto case ElectrumXJsonPropertyNames.DeserializedBlockHeader.BlockHeight;
                    case ElectrumXJsonPropertyNames.DeserializedBlockHeader.BlockHeight:
                        height = propertyValue is ElectrumXJsonValue.NumberValue numberValue &&
                                 long.TryParse(
                                     numberValue.RawJson,
                                     NumberStyles.Integer,
                                     CultureInfo.InvariantCulture,
                                     out long parsedHeight)
                            ? parsedHeight
                            : throw new JsonException("The block-header height must be a 64-bit integer.");
                        break;
                    case ElectrumXJsonPropertyNames.BlockHeaderSubscriptionResult.Hex:
                        hex = propertyValue is ElectrumXJsonValue.StringValue stringValue
                            ? stringValue.Value
                            : throw new JsonException("The block-header hex must be a string.");
                        break;
                }
            }
        }

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
