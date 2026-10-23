using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class HeaderProofResultJsonConverter : JsonConverter<ElectrumXBlockHeaderProof>
{
    public override bool HandleNull => true;

    public override ElectrumXBlockHeaderProof Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        List<string>? branch = null;
        string? header = null;
        string? rootHash = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a header proof property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The header proof property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.HeaderProofResult.Branch:
                    {
                        if (reader.TokenType != JsonTokenType.StartArray)
                        {
                            throw new JsonException("The header proof branch must be an array.");
                        }

                        branch = [];
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                        {
                            if (reader.TokenType != JsonTokenType.String)
                            {
                                throw new JsonException("A header proof branch item must be a string.");
                            }

                            branch.Add(reader.GetString()
                                       ?? throw new JsonException("A header proof branch item cannot be null."));
                        }

                        if (reader.TokenType != JsonTokenType.EndArray)
                        {
                            throw new JsonException("The header proof branch array is incomplete.");
                        }
                        break;
                    }
                case ElectrumXJsonPropertyNames.HeaderProofResult.Header:
                    header = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("The header proof header must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.HeaderProofResult.Root:
                    rootHash = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("The header proof root must be a string.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The header proof object is incomplete.");
        }

        return new ElectrumXBlockHeaderProof(
            branch ?? throw new JsonException("The header proof branch is missing."),
            header ?? throw new JsonException("The header proof header is missing."),
            rootHash ?? throw new JsonException("The header proof root is missing."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXBlockHeaderProof value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName(ElectrumXJsonPropertyNames.HeaderProofResult.Branch);
        writer.WriteStartArray();
        foreach (string item in value.Branch)
        {
            writer.WriteStringValue(item);
        }

        writer.WriteEndArray();
        writer.WriteString(ElectrumXJsonPropertyNames.HeaderProofResult.Header, value.Header);
        writer.WriteString(ElectrumXJsonPropertyNames.HeaderProofResult.Root, value.Root);
        writer.WriteEndObject();
    }
}
