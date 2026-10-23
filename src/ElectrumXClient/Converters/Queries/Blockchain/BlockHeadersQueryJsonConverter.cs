using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class BlockHeadersQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXBlockHeadersQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXBlockHeadersQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.StartHeight);
        writer.WriteNumberValue(value.Count);
        if (value.CheckpointHeight !=
            ElectrumXJsonMethodArgumentDefaultValues.ElectrumXBlockHeadersQuery.CheckpointHeight)
        {
            writer.WriteNumberValue(value.CheckpointHeight);
        }

        writer.WriteEndArray();
    }
}
