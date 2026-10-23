using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class HistoryEntryResultJsonConverter : JsonConverter<ElectrumXHistoryEntry>
{
    public override bool HandleNull => true;

    public override ElectrumXHistoryEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        long height = 0;
        string? transactionHash = null;
        long? fee = null;
        bool hasHeight = false;
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a property name.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("Expected a property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.HistoryEntryResult.Height:
                    {
                        if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt64(out long entryHeight))
                        {
                            throw new JsonException("The history height must be an integer.");
                        }
                        height = entryHeight;
                        hasHeight = true;
                        break;
                    }
                case ElectrumXJsonPropertyNames.HistoryEntryResult.TransactionHash:
                    {
                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException("The history transaction hash must be a string.");
                        }
                        transactionHash = reader.GetString();
                        break;
                    }
                case ElectrumXJsonPropertyNames.HistoryEntryResult.Fee:
                    {
                        if (reader.TokenType == JsonTokenType.Null)
                        {
                            fee = null;
                            break;
                        }

                        if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt64(out long entryFee))
                        {
                            throw new JsonException("The history fee must be an integer.");
                        }
                        fee = entryFee;
                        break;
                    }
                default:
                    {
                        if (reader.TokenType is JsonTokenType.StartArray or JsonTokenType.StartObject)
                        {
                            reader.Skip();
                        }
                        break;
                    }
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The history entry is incomplete.");
        }

        return new ElectrumXHistoryEntry(
            hasHeight ? height : throw new JsonException("The history height is missing."),
            transactionHash ?? throw new JsonException("The history transaction hash is missing."),
            fee);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXHistoryEntry value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.HistoryEntryResult.Height, value.Height);
        writer.WriteString(ElectrumXJsonPropertyNames.HistoryEntryResult.TransactionHash, value.TransactionHash);
        if (value.Fee is {} fee)
        {
            writer.WritePropertyName(ElectrumXJsonPropertyNames.HistoryEntryResult.Fee);
            writer.WriteNumberValue(fee);
        }

        writer.WriteEndObject();
    }
}
