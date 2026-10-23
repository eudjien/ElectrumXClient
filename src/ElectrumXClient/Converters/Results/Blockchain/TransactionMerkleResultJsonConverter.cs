using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class TransactionMerkleResultJsonConverter : JsonConverter<ElectrumXTransactionMerkle>
{
    public override bool HandleNull => true;

    public override ElectrumXTransactionMerkle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        long? blockHeight = null;
        List<string>? merkle = null;
        int? position = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a transaction Merkle property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The transaction Merkle property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.TransactionMerkleResult.BlockHeight:
                    blockHeight = reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long height)
                        ? height
                        : throw new JsonException("Transaction Merkle height and position must be integers.");
                    break;
                case ElectrumXJsonPropertyNames.TransactionMerkleResult.Merkle:
                    {
                        if (reader.TokenType != JsonTokenType.StartArray)
                        {
                            throw new JsonException("The transaction Merkle proof must be an array.");
                        }

                        merkle = [];
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                        {
                            if (reader.TokenType != JsonTokenType.String)
                            {
                                throw new JsonException("A transaction Merkle item must be a string.");
                            }

                            merkle.Add(reader.GetString()
                                       ?? throw new JsonException("A transaction Merkle item cannot be null."));
                        }

                        if (reader.TokenType != JsonTokenType.EndArray)
                        {
                            throw new JsonException("The transaction Merkle array is incomplete.");
                        }
                        break;
                    }
                case ElectrumXJsonPropertyNames.TransactionMerkleResult.Position:
                    if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out int transactionPosition))
                    {
                        throw new JsonException("Transaction Merkle height and position must be integers.");
                    }

                    position = transactionPosition;
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The transaction Merkle object is incomplete.");
        }

        return new ElectrumXTransactionMerkle(
            blockHeight ?? throw new JsonException("The block height is missing."),
            merkle ?? throw new JsonException("The transaction Merkle proof is missing."),
            position ?? throw new JsonException("The transaction position is missing."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXTransactionMerkle value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.TransactionMerkleResult.BlockHeight, value.BlockHeight);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.TransactionMerkleResult.Merkle);
        writer.WriteStartArray();
        foreach (string item in value.Merkle)
        {
            writer.WriteStringValue(item);
        }

        writer.WriteEndArray();
        writer.WriteNumber(ElectrumXJsonPropertyNames.TransactionMerkleResult.Position, value.Position);
        writer.WriteEndObject();
    }
}
