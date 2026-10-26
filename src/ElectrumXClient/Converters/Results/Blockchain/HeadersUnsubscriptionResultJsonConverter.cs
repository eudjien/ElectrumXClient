using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class HeadersUnsubscriptionResultJsonConverter : JsonConverter<ElectrumXHeadersUnsubscription>
{
    public override ElectrumXHeadersUnsubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new ElectrumXHeadersUnsubscription(true),
            JsonTokenType.False => new ElectrumXHeadersUnsubscription(false),
            _ => throw new JsonException("The result must be a boolean.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXHeadersUnsubscription value,
        JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.WasUnsubscribed);
    }
}
