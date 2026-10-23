using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class EstimateFeeResultJsonConverter : JsonConverter<ElectrumXEstimateFee>
{

    public override bool HandleNull => true;

    public override ElectrumXEstimateFee Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number || !reader.TryGetDecimal(out decimal fee))
        {
            throw new JsonException("The fee estimate response must be a number.");
        }

        return new ElectrumXEstimateFee(fee);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXEstimateFee value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteNumberValue(value.Value);
    }
}
