using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class TscMerkleWithTransactionHashResultJsonConverter
    : JsonConverter<ElectrumXTscMerkleWithTransactionHash>
{
    public override bool HandleNull => true;

    public override ElectrumXTscMerkleWithTransactionHash Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        bool? composite = null;
        int? index = null;
        List<string>? nodes = null;
        string? proofType = null;
        string? target = null;
        TscMerkleTargetType? targetType = null;
        string? transactionHash = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a TSC Merkle property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The TSC Merkle property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.TscMerkleResult.Composite:
                    composite = reader.TokenType switch
                    {
                        JsonTokenType.True => true,
                        JsonTokenType.False => false,
                        _ => throw new JsonException("The TSC Merkle composite flag must be a boolean.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.TscMerkleResult.Index:
                    index = reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int parsedIndex)
                        ? parsedIndex
                        : throw new JsonException("The TSC Merkle index must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.TscMerkleResult.Nodes:
                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        throw new JsonException("TSC Merkle nodes must be an array.");
                    }

                    nodes = [];
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        nodes.Add(reader.TokenType == JsonTokenType.String
                            ? reader.GetString() ?? throw new JsonException("A TSC Merkle node cannot be null.")
                            : throw new JsonException("A TSC Merkle node must be a string."));
                    }

                    if (reader.TokenType != JsonTokenType.EndArray)
                    {
                        throw new JsonException("The TSC Merkle nodes array is incomplete.");
                    }

                    break;
                case ElectrumXJsonPropertyNames.TscMerkleResult.ProofType:
                    proofType = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("TSC Merkle text properties must be strings.");
                    break;
                case ElectrumXJsonPropertyNames.TscMerkleResult.Target:
                    target = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("TSC Merkle text properties must be strings.");
                    break;
                case ElectrumXJsonPropertyNames.TscMerkleResult.TargetType:
                    string targetTypeText = reader.TokenType == JsonTokenType.String
                        ? reader.GetString() ?? throw new JsonException("The TSC Merkle target type must be a string.")
                        : throw new JsonException("The TSC Merkle target type must be a string.");
                    targetType = targetTypeText switch
                    {
                        ElectrumXJsonPropertyValues.TscMerkle.TargetType.BlockHash => TscMerkleTargetType.BlockHash,
                        ElectrumXJsonPropertyValues.TscMerkle.TargetType.BlockHeader => TscMerkleTargetType.BlockHeader,
                        ElectrumXJsonPropertyValues.TscMerkle.TargetType.MerkleRoot => TscMerkleTargetType.MerkleRoot,
                        _ => throw new JsonException($"Unsupported TSC Merkle target type '{targetTypeText}'.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.TscMerkleResult.TransactionOrId:
                    transactionHash = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("TSC Merkle text properties must be strings.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The TSC Merkle object is incomplete.");
        }

        return new ElectrumXTscMerkleWithTransactionHash(
            composite ?? throw new JsonException("The TSC Merkle composite flag is missing."),
            index ?? throw new JsonException("The TSC Merkle index is missing."),
            nodes ?? throw new JsonException("The TSC Merkle nodes are missing."),
            proofType ?? throw new JsonException("The TSC Merkle proof type is missing."),
            target ?? throw new JsonException("The TSC Merkle target is missing."),
            targetType ?? throw new JsonException("The TSC Merkle target type is missing."),
            transactionHash ?? throw new JsonException("The TSC Merkle transaction hash is missing."));
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXTscMerkleWithTransactionHash value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteBoolean(ElectrumXJsonPropertyNames.TscMerkleResult.Composite, value.Composite);
        writer.WriteNumber(ElectrumXJsonPropertyNames.TscMerkleResult.Index, value.Index);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.TscMerkleResult.Nodes);
        writer.WriteStartArray();
        foreach (string node in value.Nodes)
        {
            writer.WriteStringValue(node);
        }

        writer.WriteEndArray();
        writer.WriteString(ElectrumXJsonPropertyNames.TscMerkleResult.ProofType, value.ProofType);
        writer.WriteString(ElectrumXJsonPropertyNames.TscMerkleResult.Target, value.Target);
        writer.WriteString(
            ElectrumXJsonPropertyNames.TscMerkleResult.TargetType,
            value.TargetType switch
            {
                TscMerkleTargetType.BlockHash => ElectrumXJsonPropertyValues.TscMerkle.TargetType.BlockHash,
                TscMerkleTargetType.BlockHeader => ElectrumXJsonPropertyValues.TscMerkle.TargetType.BlockHeader,
                TscMerkleTargetType.MerkleRoot => ElectrumXJsonPropertyValues.TscMerkle.TargetType.MerkleRoot,
                _ => throw new JsonException("The TSC Merkle target type is not supported.")
            });
        writer.WriteString(
            ElectrumXJsonPropertyNames.TscMerkleResult.TransactionOrId,
            value.TransactionHash);
        writer.WriteEndObject();
    }
}
