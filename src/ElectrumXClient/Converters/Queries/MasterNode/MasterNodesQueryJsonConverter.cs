using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Masternode;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.MasterNode;

public sealed class MasterNodesQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXMasterNodesQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXMasterNodesQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStartArray();
        foreach (string payee in value.Payees)
        {
            writer.WriteStringValue(payee);
        }

        writer.WriteEndArray();
        writer.WriteEndArray();
    }
}
