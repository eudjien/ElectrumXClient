using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class BlockHeaderResultJsonConverter : JsonConverter<ElectrumXBlockHeader>
{

    public override bool HandleNull => true;

    public override ElectrumXBlockHeader Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The block header response must be a string.");
        }

        return new ElectrumXBlockHeader(reader.GetString()
                                        ?? throw new JsonException("The block header response must be a string."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXBlockHeader value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value);
    }
}
