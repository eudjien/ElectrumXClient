using NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.MasterNode;

public sealed class BroadcastMasterNodeAnnouncementResultJsonConverter
    : JsonConverter<ElectrumXBroadcastMasterNodeAnnouncement>
{

    public override bool HandleNull => true;

    public override ElectrumXBroadcastMasterNodeAnnouncement Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
        {
            throw new JsonException("The master-node announcement response must be a boolean.");
        }

        return new ElectrumXBroadcastMasterNodeAnnouncement(reader.GetBoolean());
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXBroadcastMasterNodeAnnouncement value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteBooleanValue(value.Value);
    }
}
