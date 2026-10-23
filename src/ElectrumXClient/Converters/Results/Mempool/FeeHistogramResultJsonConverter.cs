using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Mempool;

public sealed class FeeHistogramResultJsonConverter : JsonConverter<ElectrumXFeeHistogram>
{
    public override bool HandleNull => true;

    public override ElectrumXFeeHistogram Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The fee histogram response must be an array.");
        }

        var entries = new List<ElectrumXFeeHistogramEntry>();
        var entryConverter = (JsonConverter<ElectrumXFeeHistogramEntry>)options.GetConverter(typeof(ElectrumXFeeHistogramEntry));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                throw new JsonException("A fee histogram entry cannot be null.");
            }

            entries.Add(entryConverter.Read(ref reader, typeof(ElectrumXFeeHistogramEntry), options)
                        ?? throw new JsonException("A fee histogram entry cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The fee histogram response must be an array.");
        }

        return new ElectrumXFeeHistogram(entries);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXFeeHistogram value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        foreach (var entry in value.Value)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(entry.FeeRate);
            writer.WriteNumberValue(entry.VirtualSize);
            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}
