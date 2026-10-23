using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class TscMerkleWithTransactionHexQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXTscMerkleWithTransactionHexQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXTscMerkleWithTransactionHexQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        writer.WriteNumberValue(value.Height);
        writer.WriteStringValue(ElectrumXJsonPropertyValues.TscMerkle.TransactionOrId.TransactionHash);
        writer.WriteStringValue(value.TargetType.ToWireValue());

        writer.WriteEndArray();
    }
}
