using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class AddressScriptHashQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddressScriptHashQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressScriptHashQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        writer.WriteEndArray();
    }
}
