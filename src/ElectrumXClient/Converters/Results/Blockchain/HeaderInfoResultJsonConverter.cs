using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class HeaderInfoResultJsonConverter : JsonConverter<ElectrumXHeaderInfo>
{
    public override ElectrumXHeaderInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The header info result must be an object.");
        }

        long? height = null;
        string? hex = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("The header info result is incomplete.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The header info result is incomplete.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.HeaderInfoResult.Height:
                    height = FirstUseResultJsonConverter.ReadInt64(ref reader, "The header height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.HeaderInfoResult.Hex:
                    hex = FirstUseResultJsonConverter.ReadString(ref reader, "The header hex must be a string.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The header info result is incomplete.");
        }

        return new ElectrumXHeaderInfo(
            height ?? throw new JsonException("The header height is missing."),
            hex ?? throw new JsonException("The header hex is missing."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXHeaderInfo value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.HeaderInfoResult.Height, value.Height);
        writer.WriteString(ElectrumXJsonPropertyNames.HeaderInfoResult.Hex, value.Hex);
        writer.WriteEndObject();
    }
}