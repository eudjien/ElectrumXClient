using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class DoubleSpendProofUnsubscriptionResultJsonConverter : JsonConverter<ElectrumXDoubleSpendProofUnsubscription>
{
    public override ElectrumXDoubleSpendProofUnsubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new ElectrumXDoubleSpendProofUnsubscription(true),
            JsonTokenType.False => new ElectrumXDoubleSpendProofUnsubscription(false),
            _ => throw new JsonException("The result must be a boolean.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXDoubleSpendProofUnsubscription value,
        JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.WasUnsubscribed);
    }
}
