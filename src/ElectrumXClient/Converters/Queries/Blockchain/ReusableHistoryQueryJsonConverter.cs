using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class ReusableHistoryQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXReusableHistoryQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXReusableHistoryQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.FromHeight);
        writer.WriteNumberValue(value.Count);
        writer.WriteStringValue(value.RpaPrefix);
        writer.WriteEndArray();
    }
}
