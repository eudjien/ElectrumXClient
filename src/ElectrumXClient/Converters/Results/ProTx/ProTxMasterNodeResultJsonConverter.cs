using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.ProTx;

public sealed class ProTxMasterNodeResultJsonConverter : JsonConverter<ElectrumXProTxMasterNode>
{
    public override bool HandleNull => true;

    public override ElectrumXProTxMasterNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("A ProTx masternode entry must be an object.");
        }

        string? proRegTxHash = null;
        string? confirmedHash = null;
        string? service = null;
        string? operatorPublicKey = null;
        string? votingKeyId = null;
        bool? isValid = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a ProTx masternode property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("Expected a ProTx masternode property name.");
            if (!reader.Read())
            {
                throw new JsonException("Expected a ProTx masternode property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.ProTxMasterNodeResult.ProRegTxHash:
                    proRegTxHash = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxMasterNodeResult.ConfirmedHash:
                    confirmedHash = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxMasterNodeResult.Service:
                    service = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxMasterNodeResult.OperatorPublicKey:
                    operatorPublicKey = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxMasterNodeResult.VotingKeyId:
                    votingKeyId = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxMasterNodeResult.IsValid:
                    isValid = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.True => true,
                        JsonTokenType.False => false,
                        _ => throw new JsonException($"Property '{propertyName}' must be a boolean or null.")
                    };
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The ProTx masternode response is incomplete.");
        }

        return new ElectrumXProTxMasterNode(proRegTxHash, confirmedHash, service, operatorPublicKey, votingKeyId, isValid);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXProTxMasterNode value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxMasterNodeResult.ProRegTxHash, value.ProRegTxHash);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxMasterNodeResult.ConfirmedHash, value.ConfirmedHash);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxMasterNodeResult.Service, value.Service);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxMasterNodeResult.OperatorPublicKey, value.OperatorPublicKey);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxMasterNodeResult.VotingKeyId, value.VotingKeyId);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxMasterNodeResult.IsValid);
        if (value.IsValid is {} isValid)
        {
            writer.WriteBooleanValue(isValid);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteEndObject();
    }
}
