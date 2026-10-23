using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class BlockHeightResultJsonConverter : JsonConverter<ElectrumXBlockHeight>
{
    public override ElectrumXBlockHeight Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long height)
            ? new ElectrumXBlockHeight(height)
            : throw new JsonException("The block height result must be an integer.");
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXBlockHeight value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Height);
    }
}
