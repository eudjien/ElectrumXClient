using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class RelayFeeResultJsonConverter : JsonConverter<ElectrumXRelayFee>
{

    public override bool HandleNull => true;

    public override ElectrumXRelayFee Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number || !reader.TryGetDecimal(out decimal fee))
        {
            throw new JsonException("The relay fee response must be a number.");
        }

        return new ElectrumXRelayFee(fee);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXRelayFee value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteNumberValue(value.Value);
    }
}
