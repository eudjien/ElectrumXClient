using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class TransactionHeightResultJsonConverter : JsonConverter<ElectrumXTransactionHeightResult>
{
    public override bool HandleNull => true;

    public override ElectrumXTransactionHeightResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetInt64(out long height) => new ElectrumXTransactionHeightResult(height),
            JsonTokenType.Null => new ElectrumXTransactionHeightResult(null),
            _ => throw new JsonException("The result must be an integer or null.")
        };
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXTransactionHeightResult value, JsonSerializerOptions options)
    {
        if (value.Height is {} height)
        {
            writer.WriteNumberValue(height);
            return;
        }

        writer.WriteNullValue();
    }
}
