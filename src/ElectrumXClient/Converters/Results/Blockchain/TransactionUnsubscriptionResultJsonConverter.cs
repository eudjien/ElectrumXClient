using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class TransactionUnsubscriptionResultJsonConverter : JsonConverter<ElectrumXTransactionUnsubscription>
{
    public override ElectrumXTransactionUnsubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new ElectrumXTransactionUnsubscription(true),
            JsonTokenType.False => new ElectrumXTransactionUnsubscription(false),
            _ => throw new JsonException("The result must be a boolean.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXTransactionUnsubscription value,
        JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.WasUnsubscribed);
    }
}
