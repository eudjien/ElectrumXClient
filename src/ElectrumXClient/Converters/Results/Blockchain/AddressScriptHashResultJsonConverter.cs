using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class AddressScriptHashResultJsonConverter : JsonConverter<ElectrumXAddressScriptHash>
{
    public override ElectrumXAddressScriptHash Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new ElectrumXAddressScriptHash(reader.GetString() ?? throw new JsonException("The script hash cannot be null."))
            : throw new JsonException("The script hash result must be a string.");
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressScriptHash value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ScriptHash);
    }
}
