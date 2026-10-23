using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Mempool;

public sealed class MempoolEntryResultJsonConverter : JsonConverter<ElectrumXMempoolEntry>
{
    public override bool HandleNull => true;

    public override ElectrumXMempoolEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        int? height = null;
        string? transactionHash = null;
        long? fee = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a mempool entry property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The mempool entry property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.MempoolEntryResult.Height:
                    height = reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int entryHeight)
                        ? entryHeight
                        : throw new JsonException("The mempool height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.MempoolEntryResult.TransactionHash:
                    transactionHash = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("The mempool transaction hash must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.MempoolEntryResult.Fee:
                    fee = reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long entryFee)
                        ? entryFee
                        : throw new JsonException("The mempool entry fee must be an integer.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The mempool entry object is incomplete.");
        }

        return new ElectrumXMempoolEntry(
            height ?? throw new JsonException("The mempool entry height is missing."),
            transactionHash ?? throw new JsonException("The mempool transaction hash is missing."),
            fee ?? throw new JsonException("The mempool entry fee is missing."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXMempoolEntry value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.MempoolEntryResult.Height, value.Height);
        writer.WriteString(ElectrumXJsonPropertyNames.MempoolEntryResult.TransactionHash, value.TransactionHash);
        writer.WriteNumber(ElectrumXJsonPropertyNames.MempoolEntryResult.Fee, value.Fee);
        writer.WriteEndObject();
    }
}
