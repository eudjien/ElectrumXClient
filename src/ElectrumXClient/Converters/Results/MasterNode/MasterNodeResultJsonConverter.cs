using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.MasterNode;

public sealed class MasterNodeResultJsonConverter : JsonConverter<ElectrumXMasterNode>
{
    public override bool HandleNull => true;

    public override ElectrumXMasterNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("A masternode entry must be an object.");
        }

        string? vin = null;
        string? status = null;
        int? protocol = null;
        string? payee = null;
        string? lastSeen = null;
        long? activeSeconds = null;
        string? lastPaidTime = null;
        long? lastPaidBlock = null;
        string? ip = null;
        long? paymentPosition = null;
        bool? inSelection = null;
        decimal? balance = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a masternode property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("A masternode property name cannot be null.");
            if (!reader.Read())
            {
                throw new JsonException("Expected a masternode property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.MasterNodeResult.Vin:
                    vin = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.Status:
                    status = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.Protocol:
                    protocol = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt32(out int value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.Payee:
                    payee = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.LastSeen:
                    lastSeen = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.ActiveSeconds:
                    activeSeconds = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.LastPaidTime:
                    lastPaidTime = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.LastPaidBlock:
                    lastPaidBlock = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.Ip:
                    ip = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.PaymentPosition:
                    paymentPosition = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.InSelection:
                    inSelection = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.True => true,
                        JsonTokenType.False => false,
                        _ => throw new JsonException($"Property '{propertyName}' must be a boolean or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.MasterNodeResult.Balance:
                    balance = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetDecimal(out decimal value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be a number or null.")
                    };
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The masternode response is incomplete.");
        }

        return new ElectrumXMasterNode(
            vin, status, protocol, payee, lastSeen, activeSeconds, lastPaidTime, lastPaidBlock, ip,
            paymentPosition, inSelection, balance);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXMasterNode value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.MasterNodeResult.Vin, value.Vin);
        writer.WriteString(ElectrumXJsonPropertyNames.MasterNodeResult.Status, value.Status);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.MasterNodeResult.Protocol);
        if (value.Protocol is {} protocol)
        {
            writer.WriteNumberValue(protocol);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.MasterNodeResult.Payee, value.Payee);
        writer.WriteString(ElectrumXJsonPropertyNames.MasterNodeResult.LastSeen, value.LastSeen);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.MasterNodeResult.ActiveSeconds);
        if (value.ActiveSeconds is {} activeSeconds)
        {
            writer.WriteNumberValue(activeSeconds);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.MasterNodeResult.LastPaidTime, value.LastPaidTime);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.MasterNodeResult.LastPaidBlock);
        if (value.LastPaidBlock is {} lastPaidBlock)
        {
            writer.WriteNumberValue(lastPaidBlock);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.MasterNodeResult.Ip, value.Ip);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.MasterNodeResult.PaymentPosition);
        if (value.PaymentPosition is {} paymentPosition)
        {
            writer.WriteNumberValue(paymentPosition);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.MasterNodeResult.InSelection);
        if (value.InSelection is {} inSelection)
        {
            writer.WriteBooleanValue(inSelection);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.MasterNodeResult.Balance);
        if (value.Balance is {} balance)
        {
            writer.WriteNumberValue(balance);
        }
        else
        {
            writer.WriteNullValue();
        }
        writer.WriteEndObject();
    }
}
