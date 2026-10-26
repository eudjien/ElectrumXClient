using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class StringListResultJsonConverter : JsonConverter<ElectrumXStringList>
{
    public override ElectrumXStringList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The result must be an array of strings.");
        }

        var values = new List<string>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("A result array item must be a string.");
            }

            values.Add(reader.GetString() ?? throw new JsonException("A result array item cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The result array is incomplete.");
        }

        return new ElectrumXStringList(values);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXStringList value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (string item in value.Value)
        {
            writer.WriteStringValue(item);
        }

        writer.WriteEndArray();
    }
}
