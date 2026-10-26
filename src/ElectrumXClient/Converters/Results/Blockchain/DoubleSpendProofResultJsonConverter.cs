using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class DoubleSpendProofResultJsonConverter : JsonConverter<ElectrumXDoubleSpendProof>
{
    public override ElectrumXDoubleSpendProof Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The double-spend proof response must be an object.");
        }

        string? doubleSpendProofId = null;
        string? transactionId = null;
        string? hex = null;
        ElectrumXDoubleSpendProofOutpoint? outpoint = null;
        List<string>? descendants = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a double-spend proof property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The double-spend proof property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.DoubleSpendProofResult.DoubleSpendProofId:
                    doubleSpendProofId = reader.TokenType == JsonTokenType.String
                        ? reader.GetString() ?? throw new JsonException("The double-spend proof ID must be a string.")
                        : throw new JsonException("The double-spend proof ID must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.DoubleSpendProofResult.TransactionId:
                    transactionId = reader.TokenType == JsonTokenType.String
                        ? reader.GetString() ?? throw new JsonException("The double-spend proof transaction ID must be a string.")
                        : throw new JsonException("The double-spend proof transaction ID must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.DoubleSpendProofResult.Hex:
                    hex = reader.TokenType == JsonTokenType.String
                        ? reader.GetString() ?? throw new JsonException("The double-spend proof hex must be a string.")
                        : throw new JsonException("The double-spend proof hex must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.DoubleSpendProofResult.Outpoint:
                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException("The double-spend proof outpoint must be an object.");
                    }

                    string? outpointTransactionId = null;
                    int? outputIndex = null;
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
                        {
                            throw new JsonException("The double-spend proof outpoint is incomplete.");
                        }

                        switch (reader.GetString())
                        {
                            case ElectrumXJsonPropertyNames.DoubleSpendProofResult.TransactionId:
                                outpointTransactionId = reader.TokenType == JsonTokenType.String
                                    ? reader.GetString() ?? throw new JsonException("The outpoint transaction ID must be a string.")
                                    : throw new JsonException("The outpoint transaction ID must be a string.");
                                break;
                            case ElectrumXJsonPropertyNames.DoubleSpendProofResult.OutputIndex:
                                outputIndex = reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int parsedIndex)
                                    ? parsedIndex
                                    : throw new JsonException("The outpoint output index must be an integer.");
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }

                    if (reader.TokenType != JsonTokenType.EndObject)
                    {
                        throw new JsonException("The double-spend proof outpoint is incomplete.");
                    }

                    outpoint = new ElectrumXDoubleSpendProofOutpoint(
                        outpointTransactionId ?? throw new JsonException("The outpoint transaction ID is missing."),
                        outputIndex ?? throw new JsonException("The outpoint output index is missing."));
                    break;
                case ElectrumXJsonPropertyNames.DoubleSpendProofResult.Descendants:
                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        throw new JsonException("The double-spend proof descendants must be an array.");
                    }

                    descendants = [];
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        descendants.Add(
                            reader.TokenType == JsonTokenType.String
                                ? reader.GetString() ?? throw new JsonException("A double-spend proof descendant must be a string.")
                                : throw new JsonException("A double-spend proof descendant must be a string."));
                    }

                    if (reader.TokenType != JsonTokenType.EndArray)
                    {
                        throw new JsonException("The double-spend proof descendants array is incomplete.");
                    }
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The double-spend proof object is incomplete.");
        }

        return new ElectrumXDoubleSpendProof(
            doubleSpendProofId ?? throw new JsonException("The double-spend proof ID is missing."),
            transactionId ?? throw new JsonException("The double-spend proof transaction ID is missing."),
            hex ?? throw new JsonException("The double-spend proof hex is missing."),
            outpoint ?? throw new JsonException("The double-spend proof outpoint is missing."),
            descendants ?? throw new JsonException("The double-spend proof descendants are missing."));
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXDoubleSpendProof value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.DoubleSpendProofResult.DoubleSpendProofId, value.DoubleSpendProofId);
        writer.WriteString(ElectrumXJsonPropertyNames.DoubleSpendProofResult.TransactionId, value.TransactionId);
        writer.WriteString(ElectrumXJsonPropertyNames.DoubleSpendProofResult.Hex, value.Hex);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.DoubleSpendProofResult.Outpoint);
        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.DoubleSpendProofResult.TransactionId, value.Outpoint.TransactionId);
        writer.WriteNumber(ElectrumXJsonPropertyNames.DoubleSpendProofResult.OutputIndex, value.Outpoint.OutputIndex);
        writer.WriteEndObject();
        writer.WritePropertyName(ElectrumXJsonPropertyNames.DoubleSpendProofResult.Descendants);
        writer.WriteStartArray();
        foreach (string descendant in value.Descendants)
        {
            writer.WriteStringValue(descendant);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}
