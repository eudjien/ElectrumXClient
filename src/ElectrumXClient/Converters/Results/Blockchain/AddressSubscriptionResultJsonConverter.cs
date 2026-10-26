using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class AddressSubscriptionResultJsonConverter : JsonConverter<ElectrumXAddressSubscription>
{
    public override bool HandleNull => true;

    public override ElectrumXAddressSubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new ElectrumXAddressSubscription(reader.GetString()),
            JsonTokenType.Null => new ElectrumXAddressSubscription(null),
            _ => throw new JsonException("The result must be a string or null.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXAddressSubscription value,
        JsonSerializerOptions options)
    {
        if (value.Status is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Status);
    }
}
