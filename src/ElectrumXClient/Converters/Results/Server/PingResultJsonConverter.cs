using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class PingResultJsonConverter : JsonConverter<ElectrumXPing>
{
    public override bool HandleNull => true;

    public override ElectrumXPing Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new ElectrumXPing();
        }

        if (reader.TokenType != JsonTokenType.StartObject || !reader.Read() || reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The ping response must be null.");
        }

        return new ElectrumXPing();
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXPing value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteNullValue();
    }
}
