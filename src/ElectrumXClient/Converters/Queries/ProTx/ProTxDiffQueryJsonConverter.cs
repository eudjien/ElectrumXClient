using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.ProTx;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.ProTx;

public sealed class ProTxDiffQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXProTxDiffQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXProTxDiffQuery? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.BaseHeight);
        writer.WriteNumberValue(value.Height);

        writer.WriteEndArray();
    }
}
