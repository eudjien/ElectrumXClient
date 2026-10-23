using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Server;

public sealed class ServerVersionQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXServerVersionQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXServerVersionQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.ClientName);
        writer.WriteStartArray();
        writer.WriteStringValue(value.ProtocolMinimum);
        writer.WriteStringValue(value.ProtocolMaximum);
        writer.WriteEndArray();

        writer.WriteEndArray();
    }
}
