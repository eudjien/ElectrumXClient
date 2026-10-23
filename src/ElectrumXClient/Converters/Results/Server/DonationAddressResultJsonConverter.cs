using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class DonationAddressResultJsonConverter : JsonConverter<ElectrumXDonationAddress>
{

    public override bool HandleNull => true;

    public override ElectrumXDonationAddress Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The donation address response must be a string.");
        }

        return new ElectrumXDonationAddress(reader.GetString()
                                            ?? throw new JsonException("The donation address response must be a string."));
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXDonationAddress value,
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
