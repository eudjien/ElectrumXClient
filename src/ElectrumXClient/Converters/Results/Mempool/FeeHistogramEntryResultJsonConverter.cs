using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Mempool;

public sealed class FeeHistogramEntryResultJsonConverter : JsonConverter<ElectrumXFeeHistogramEntry>
{
    public override bool HandleNull => true;

    public override ElectrumXFeeHistogramEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray ||
            !reader.Read() ||
            reader.TokenType != JsonTokenType.Number ||
            !reader.TryGetDecimal(out decimal feeRate) ||
            !reader.Read() ||
            reader.TokenType != JsonTokenType.Number ||
            !reader.TryGetInt64(out long virtualSize) ||
            !reader.Read() ||
            reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("A fee histogram entry must contain a fee rate and virtual size.");
        }

        return new ElectrumXFeeHistogramEntry(feeRate, virtualSize);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXFeeHistogramEntry value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.FeeRate);
        writer.WriteNumberValue(value.VirtualSize);
        writer.WriteEndArray();
    }
}
