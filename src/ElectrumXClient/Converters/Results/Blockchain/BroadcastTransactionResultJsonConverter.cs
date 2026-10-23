using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class BroadcastTransactionResultJsonConverter
    : JsonConverter<ElectrumXBroadcastTransaction>
{

    public override bool HandleNull => true;

    public override ElectrumXBroadcastTransaction Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The broadcast transaction response must be a string.");
        }

        return new ElectrumXBroadcastTransaction(reader.GetString()
                                                 ?? throw new JsonException("The broadcast transaction response must be a string."));
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXBroadcastTransaction value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value);
    }
}
