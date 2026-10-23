using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class SubscribeToHeadersQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXSubscribeToHeadersQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXSubscribeToHeadersQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();

        if (value.Raw is not null)
        {
            writer.WriteBooleanValue(value.Raw.Value);
        }

        writer.WriteEndArray();
    }
}
