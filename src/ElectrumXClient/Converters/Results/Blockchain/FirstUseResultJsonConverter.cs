using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class FirstUseResultJsonConverter : JsonConverter<ElectrumXFirstUse>
{
    public override ElectrumXFirstUse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The first-use result must be an object.");
        }

        string? blockHash = null;
        long? height = null;
        string? transactionHash = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("The first-use result is incomplete.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The first-use result is incomplete.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.FirstUseResult.BlockHash:
                    blockHash = reader.TokenType == JsonTokenType.String
                        ? reader.GetString() ?? throw new JsonException("The first-use block hash must be a string.")
                        : throw new JsonException("The first-use block hash must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.FirstUseResult.Height:
                    height = reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long blockHeight)
                        ? blockHeight
                        : throw new JsonException("The first-use block height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.FirstUseResult.TransactionHash:
                    transactionHash = reader.TokenType == JsonTokenType.String
                        ? reader.GetString() ?? throw new JsonException("The first-use transaction hash must be a string.")
                        : throw new JsonException("The first-use transaction hash must be a string.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The first-use result is incomplete.");
        }

        return new ElectrumXFirstUse(
            blockHash ?? throw new JsonException("The first-use block hash is missing."),
            height ?? throw new JsonException("The first-use block height is missing."),
            transactionHash ?? throw new JsonException("The first-use transaction hash is missing."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXFirstUse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.FirstUseResult.BlockHash, value.BlockHash);
        writer.WriteNumber(ElectrumXJsonPropertyNames.FirstUseResult.Height, value.Height);
        writer.WriteString(ElectrumXJsonPropertyNames.FirstUseResult.TransactionHash, value.TransactionHash);
        writer.WriteEndObject();
    }
}
