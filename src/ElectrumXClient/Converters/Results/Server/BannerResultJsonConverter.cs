using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class BannerResultJsonConverter : JsonConverter<ElectrumXBanner>
{

    public override bool HandleNull => true;

    public override ElectrumXBanner Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The server banner must be a string.");
        }

        return new ElectrumXBanner(reader.GetString() ?? throw new JsonException("The server banner must be a string."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXBanner value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value);
    }
}
