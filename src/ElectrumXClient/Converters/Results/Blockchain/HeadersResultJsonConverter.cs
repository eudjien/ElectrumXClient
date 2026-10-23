using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class HeadersResultJsonConverter : JsonConverter<ElectrumXBlockHeaders>
{
    public override bool HandleNull => true;

    public override ElectrumXBlockHeaders Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        int? count = null;
        string? hex = null;
        IReadOnlyList<string>? headers = null;
        int? maximum = null;
        string? rootHash = null;
        IReadOnlyList<string>? branch = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a headers property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The headers property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.HeadersResult.Count:
                    if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out int headersCount))
                    {
                        throw new JsonException("The headers count must be an integer.");
                    }

                    count = headersCount;
                    break;
                case ElectrumXJsonPropertyNames.HeadersResult.Hex:
                    hex = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("The headers hex cannot be null.");
                    break;
                case ElectrumXJsonPropertyNames.HeadersResult.Headers:
                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        throw new JsonException("The headers must be an array of hexadecimal strings.");
                    }

                    var headerItems = new List<string>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException("A block header must be a string.");
                        }

                        headerItems.Add(reader.GetString()
                                        ?? throw new JsonException("A block header cannot be null."));
                    }

                    if (reader.TokenType != JsonTokenType.EndArray)
                    {
                        throw new JsonException("The headers array is incomplete.");
                    }

                    headers = headerItems;
                    break;
                case ElectrumXJsonPropertyNames.HeadersResult.Maximum:
                    maximum = reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int headersMaximum)
                        ? headersMaximum
                        : throw new JsonException("The headers maximum must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.HeadersResult.Root:
                    rootHash = reader.TokenType == JsonTokenType.Null
                        ? null
                        : reader.TokenType == JsonTokenType.String
                            ? reader.GetString()
                            : throw new JsonException("The checkpoint root must be a string or null.");
                    break;
                case ElectrumXJsonPropertyNames.HeadersResult.Branch:
                    {
                        switch (reader.TokenType)
                        {
                            case JsonTokenType.Null:
                                branch = null;
                                break;
                            case JsonTokenType.StartArray:
                                {
                                    var branchItems = new List<string>();
                                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                                    {
                                        if (reader.TokenType != JsonTokenType.String)
                                        {
                                            throw new JsonException("A header branch item must be a string.");
                                        }

                                        branchItems.Add(reader.GetString()
                                                        ?? throw new JsonException("A header branch item cannot be null."));
                                    }

                                    if (reader.TokenType != JsonTokenType.EndArray)
                                    {
                                        throw new JsonException("The header branch array is incomplete.");
                                    }

                                    branch = branchItems;
                                    break;
                                }
                            default:
                                throw new JsonException("The header branch must be an array or null.");
                        }
                        break;
                    }
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The headers object is incomplete.");
        }

        if (hex is null && headers is null)
        {
            throw new JsonException("The headers data is missing.");
        }

        string combinedHeaders = hex ?? string.Concat(headers ?? Array.Empty<string>());

        return new ElectrumXBlockHeaders(
            count ?? throw new JsonException("The headers count is missing."),
            combinedHeaders,
            maximum ?? throw new JsonException("The headers maximum is missing."),
            rootHash,
            branch,
            headers);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXBlockHeaders value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.HeadersResult.Count, value.Count);
        if (value.Headers is null)
        {
            writer.WriteString(ElectrumXJsonPropertyNames.HeadersResult.Hex, value.Hex);
        }
        else
        {
            writer.WritePropertyName(ElectrumXJsonPropertyNames.HeadersResult.Headers);
            writer.WriteStartArray();
            foreach (string header in value.Headers)
            {
                writer.WriteStringValue(header);
            }

            writer.WriteEndArray();
        }
        writer.WriteNumber(ElectrumXJsonPropertyNames.HeadersResult.Maximum, value.Maximum);
        if (value.Root is not null && value.Branch is not null)
        {
            writer.WriteString(ElectrumXJsonPropertyNames.HeadersResult.Root, value.Root);
            writer.WritePropertyName(ElectrumXJsonPropertyNames.HeadersResult.Branch);
            writer.WriteStartArray();
            foreach (string item in value.Branch)
            {
                writer.WriteStringValue(item);
            }

            writer.WriteEndArray();
        }
        writer.WriteEndObject();
    }
}
