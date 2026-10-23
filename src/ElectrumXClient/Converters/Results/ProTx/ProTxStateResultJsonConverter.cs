using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.ProTx;

public sealed class ProTxStateResultJsonConverter : JsonConverter<ElectrumXProTxState>
{
    public override bool HandleNull => true;

    public override ElectrumXProTxState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The ProTx state must be an object.");
        }

        long? registeredHeight = null;
        long? lastPaidHeight = null;
        long? poSePenalty = null;
        long? poSeRevivedHeight = null;
        long? poSeBanHeight = null;
        long? revocationReason = null;
        string? ownerKeyId = null;
        string? operatorPublicKey = null;
        string? votingKeyId = null;
        string? ownerKeyAddress = null;
        string? votingKeyAddress = null;
        string? address = null;
        string? payoutAddress = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a ProTx state property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("Expected a ProTx state property name.");
            if (!reader.Read())
            {
                throw new JsonException("Expected a ProTx state property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.ProTxStateResult.RegisteredHeight:
                    registeredHeight = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.LastPaidHeight:
                    lastPaidHeight = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.PoSePenalty:
                    poSePenalty = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.PoSeRevivedHeight:
                    poSeRevivedHeight = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.PoSeBanHeight:
                    poSeBanHeight = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.RevocationReason:
                    revocationReason = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.OwnerKeyId:
                    ownerKeyId = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.OperatorPublicKey:
                    operatorPublicKey = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.VotingKeyId:
                    votingKeyId = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.OwnerKeyAddress:
                    ownerKeyAddress = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.VotingKeyAddress:
                    votingKeyAddress = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.Address:
                    address = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxStateResult.PayoutAddress:
                    payoutAddress = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The ProTx state is incomplete.");
        }

        return new ElectrumXProTxState(
            registeredHeight, lastPaidHeight, poSePenalty, poSeRevivedHeight, poSeBanHeight, revocationReason,
            ownerKeyId, operatorPublicKey, votingKeyId, ownerKeyAddress, votingKeyAddress, address, payoutAddress);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXProTxState value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxStateResult.RegisteredHeight);
        if (value.RegisteredHeight is {} registeredHeight)
        {
            writer.WriteNumberValue(registeredHeight);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxStateResult.LastPaidHeight);
        if (value.LastPaidHeight is {} lastPaidHeight)
        {
            writer.WriteNumberValue(lastPaidHeight);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxStateResult.PoSePenalty);
        if (value.PoSePenalty is {} poSePenalty)
        {
            writer.WriteNumberValue(poSePenalty);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxStateResult.PoSeRevivedHeight);
        if (value.PoSeRevivedHeight is {} poSeRevivedHeight)
        {
            writer.WriteNumberValue(poSeRevivedHeight);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxStateResult.PoSeBanHeight);
        if (value.PoSeBanHeight is {} poSeBanHeight)
        {
            writer.WriteNumberValue(poSeBanHeight);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxStateResult.RevocationReason);
        if (value.RevocationReason is {} revocationReason)
        {
            writer.WriteNumberValue(revocationReason);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.ProTxStateResult.OwnerKeyId, value.OwnerKeyId);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxStateResult.OperatorPublicKey, value.OperatorPublicKey);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxStateResult.VotingKeyId, value.VotingKeyId);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxStateResult.OwnerKeyAddress, value.OwnerKeyAddress);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxStateResult.VotingKeyAddress, value.VotingKeyAddress);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxStateResult.Address, value.Address);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxStateResult.PayoutAddress, value.PayoutAddress);
        writer.WriteEndObject();
    }
}
