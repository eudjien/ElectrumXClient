using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class BlockHeaderChunkResultJsonConverter : JsonConverter<ElectrumXBlockHeaderChunk>
{
    public override ElectrumXBlockHeaderChunk Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new ElectrumXBlockHeaderChunk(reader.GetString() ?? throw new JsonException("The block-header chunk cannot be null."))
            : throw new JsonException("The block-header chunk result must be a string.");
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXBlockHeaderChunk value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
