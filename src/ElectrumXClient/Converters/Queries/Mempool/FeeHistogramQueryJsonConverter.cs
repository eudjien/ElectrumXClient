using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Mempool;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Mempool;

public sealed class FeeHistogramQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXFeeHistogramQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXFeeHistogramQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();

        writer.WriteEndArray();
    }
}
