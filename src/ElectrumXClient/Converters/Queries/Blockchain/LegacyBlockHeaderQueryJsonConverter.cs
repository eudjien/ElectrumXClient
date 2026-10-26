using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class LegacyBlockHeaderQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXLegacyBlockHeaderQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXLegacyBlockHeaderQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.Height);
        writer.WriteEndArray();
    }
}
