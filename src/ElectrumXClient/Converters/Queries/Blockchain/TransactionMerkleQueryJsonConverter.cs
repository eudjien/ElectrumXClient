using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class TransactionMerkleQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXTransactionMerkleQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXTransactionMerkleQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        if (value.Height is {} height)
        {
            writer.WriteNumberValue(height);
        }

        writer.WriteEndArray();
    }
}
