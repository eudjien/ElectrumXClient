using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class ConfirmedTransactionBlockHashResultJsonConverter : JsonConverter<ElectrumXConfirmedTransactionBlockHash>
{
    public override ElectrumXConfirmedTransactionBlockHash Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The confirmed transaction block result must be an object.");
        }

        string? blockHash = null;
        long? blockHeight = null;
        string? blockHeader = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("The confirmed transaction block result is incomplete.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The confirmed transaction block result is incomplete.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHash:
                    blockHash = FirstUseResultJsonConverter.ReadString(ref reader, "The block hash must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHeight:
                    blockHeight = FirstUseResultJsonConverter.ReadInt64(ref reader, "The block height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHeader:
                    blockHeader = reader.TokenType == JsonTokenType.Null
                        ? null
                        : FirstUseResultJsonConverter.ReadString(ref reader, "The block header must be a string.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The confirmed transaction block result is incomplete.");
        }

        return new ElectrumXConfirmedTransactionBlockHash(
            blockHash ?? throw new JsonException("The confirmed transaction block hash is missing."),
            blockHeight ?? throw new JsonException("The confirmed transaction block height is missing."),
            blockHeader);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXConfirmedTransactionBlockHash value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHash, value.BlockHash);
        writer.WriteNumber(ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHeight, value.BlockHeight);
        if (value.BlockHeader is not null)
        {
            writer.WriteString(ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHeader, value.BlockHeader);
        }

        writer.WriteEndObject();
    }
}