using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class UtxoAddressResultJsonConverter : JsonConverter<ElectrumXUtxoAddress>
{
    public override bool HandleNull => true;

    public override ElectrumXUtxoAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new ElectrumXUtxoAddress(reader.GetString()),
            JsonTokenType.Null => new ElectrumXUtxoAddress(null),
            _ => throw new JsonException("The UTXO address result must be a string or null.")
        };
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXUtxoAddress value, JsonSerializerOptions options)
    {
        if (value.Address is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Address);
    }
}
