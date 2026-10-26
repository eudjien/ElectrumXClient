using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Masternode;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.MasterNode;

public sealed class BroadcastMasterNodeAnnouncementQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXBroadcastMasterNodeAnnouncementQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXBroadcastMasterNodeAnnouncementQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.SignedAnnouncement);

        writer.WriteEndArray();
    }
}
