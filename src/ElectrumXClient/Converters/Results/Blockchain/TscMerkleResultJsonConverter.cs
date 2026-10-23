using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public abstract class TscMerkleResultJsonConverter<TResponse> : JsonConverter<TResponse>
    where TResponse : ElectrumXTscMerkle
{
    public override bool HandleNull => true;

    protected abstract TResponse Create(
        bool composite,
        int index,
        IReadOnlyList<string> nodes,
        string proofType,
        string target,
        TscMerkleTargetType targetType,
        string transactionOrId);

    protected abstract string GetTransactionOrId(TResponse response);

    public override TResponse Read(
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
        string? transactionOrId = null;

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
                    if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt32(out int merkleIndex))
                    {
                        throw new JsonException("The TSC Merkle index must be an integer.");
                    }

                    index = merkleIndex;
                    break;
                case ElectrumXJsonPropertyNames.TscMerkleResult.Nodes:
                    {
                        if (reader.TokenType != JsonTokenType.StartArray)
                        {
                            throw new JsonException("TSC Merkle nodes must be an array.");
                        }

                        nodes = [];
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                        {
                            if (reader.TokenType != JsonTokenType.String)
                            {
                                throw new JsonException("A TSC Merkle node must be a string.");
                            }

                            nodes.Add(reader.GetString()
                                      ?? throw new JsonException("A TSC Merkle node cannot be null."));
                        }

                        if (reader.TokenType != JsonTokenType.EndArray)
                        {
                            throw new JsonException("The TSC Merkle nodes array is incomplete.");
                        }
                        break;
                    }
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
                        ? reader.GetString()
                          ?? throw new JsonException("The TSC Merkle target type must be a string.")
                        : throw new JsonException("The TSC Merkle target type must be a string.");
                    targetType = TscMerkleTargetTypeExtensions.TryParse(targetTypeText, out var parsedTargetType)
                        ? parsedTargetType
                        : throw new JsonException($"Unsupported TSC Merkle target type '{targetTypeText}'.");
                    break;
                case ElectrumXJsonPropertyNames.TscMerkleResult.TransactionOrId:
                    transactionOrId = reader.TokenType == JsonTokenType.String
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

        var targetTypeValue = targetType
                              ?? throw new JsonException("The TSC Merkle target type is missing.");

        return Create(
            composite ?? throw new JsonException("The TSC Merkle composite flag is missing."),
            index ?? throw new JsonException("The TSC Merkle index is missing."),
            nodes ?? throw new JsonException("The TSC Merkle nodes are missing."),
            proofType ?? throw new JsonException("The TSC Merkle proof type is missing."),
            target ?? throw new JsonException("The TSC Merkle target is missing."),
            targetTypeValue,
            transactionOrId ?? throw new JsonException("The TSC Merkle transaction or ID is missing."));
    }

    public override void Write(Utf8JsonWriter writer, TResponse value, JsonSerializerOptions options)
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
            value.TargetType.ToWireValue());
        writer.WriteString(ElectrumXJsonPropertyNames.TscMerkleResult.TransactionOrId, GetTransactionOrId(value));
        writer.WriteEndObject();
    }
}
