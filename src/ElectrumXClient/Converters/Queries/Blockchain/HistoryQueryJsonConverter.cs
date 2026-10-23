using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class HistoryQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXHistoryQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXHistoryQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.ScriptHash);

        if (value.FromHeight != 0 || value.ToHeight != -1)
        {
            writer.WriteNumberValue(value.FromHeight);
            writer.WriteNumberValue(value.ToHeight);
        }

        writer.WriteEndArray();
    }
}
