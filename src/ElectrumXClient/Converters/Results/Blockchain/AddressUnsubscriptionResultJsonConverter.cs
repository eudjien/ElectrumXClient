using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class AddressUnsubscriptionResultJsonConverter : JsonConverter<ElectrumXAddressUnsubscription>
{
    public override ElectrumXAddressUnsubscription Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new ElectrumXAddressUnsubscription(true),
            JsonTokenType.False => new ElectrumXAddressUnsubscription(false),
            _ => throw new JsonException("The result must be a boolean.")
        };
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressUnsubscription value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.WasUnsubscribed);
    }
}
