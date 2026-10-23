using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class IdFromPositionResultJsonConverter : JsonConverter<ElectrumXIdFromPosition>
{
    public override bool HandleNull => true;

    public override ElectrumXIdFromPosition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return new ElectrumXIdFromPosition(reader.GetString()
                                               ?? throw new JsonException("The transaction hash cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON string or object.");
        }

        var merkle = new List<string>();
        string? transactionHash = null;
        bool hasMerkle = false;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected an ID-from-position property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The ID-from-position property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.IdFromPositionResult.TransactionHash:
                    if (reader.TokenType != JsonTokenType.String)
                    {
                        throw new JsonException("The transaction hash must be a string.");
                    }

                    transactionHash = reader.GetString()
                                      ?? throw new JsonException("The transaction hash cannot be null.");
                    break;

                case ElectrumXJsonPropertyNames.IdFromPositionResult.Merkle:
                    merkle.Clear();
                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        throw new JsonException("The Merkle proof must be an array.");
                    }

                    hasMerkle = true;
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException("A Merkle proof item must be a string.");
                        }

                        merkle.Add(reader.GetString()
                                   ?? throw new JsonException("A Merkle proof item cannot be null."));
                    }

                    if (reader.TokenType != JsonTokenType.EndArray)
                    {
                        throw new JsonException("The Merkle proof array is incomplete.");
                    }

                    break;

                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The ID-from-position object is incomplete.");
        }

        if (transactionHash is null || !hasMerkle)
        {
            throw new JsonException("The ID-from-position response must contain tx_hash and merkle.");
        }

        return new ElectrumXIdFromPosition(transactionHash, merkle);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXIdFromPosition value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        if (value.Merkle is null)
        {
            writer.WriteStringValue(value.TransactionHash);
            return;
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.IdFromPositionResult.TransactionHash, value.TransactionHash);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.IdFromPositionResult.Merkle);
        writer.WriteStartArray();
        foreach (string item in value.Merkle)
        {
            writer.WriteStringValue(item);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}
