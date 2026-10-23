using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class AddPeerResultJsonConverter : JsonConverter<ElectrumXAddPeer>
{
    public override bool HandleNull => true;

    public override ElectrumXAddPeer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
        {
            throw new JsonException("The add-peer response must be a boolean.");
        }

        return new ElectrumXAddPeer(reader.GetBoolean());
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXAddPeer value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteBooleanValue(value.Value);
    }
}
