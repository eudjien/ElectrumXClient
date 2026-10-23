using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class RawTransactionResultJsonConverter : JsonConverter<ElectrumXRawTransaction>
{

    public override bool HandleNull => true;

    public override ElectrumXRawTransaction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The raw transaction response must be a string.");
        }

        return new ElectrumXRawTransaction(reader.GetString()
                                           ?? throw new JsonException("The raw transaction response cannot be null."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXRawTransaction value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value);
    }
}
