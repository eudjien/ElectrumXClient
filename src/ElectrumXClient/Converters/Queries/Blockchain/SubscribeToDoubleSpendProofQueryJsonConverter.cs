using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class SubscribeToDoubleSpendProofQueryJsonConverter
    : ElectrumXQueryJsonConverter<ElectrumXSubscribeToDoubleSpendProofQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXSubscribeToDoubleSpendProofQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        writer.WriteEndArray();
    }
}
