using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class VerboseTransactionInputResultJsonConverter : JsonConverter<ElectrumXVerboseTransactionInput>
{

    private static readonly ElectrumXJsonValueResultJsonConverter JsonValueConverter = new();
    public override bool HandleNull => true;

    public override ElectrumXVerboseTransactionInput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("A verbose transaction input must be an object.");
        }

        string? transactionId = null;
        long? outputIndex = null;
        ElectrumXVerboseScriptSig? scriptSignature = null;
        ulong? sequence = null;
        string? coinbase = null;
        var additionalProperties = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);
        var scriptConverter = (JsonConverter<ElectrumXVerboseScriptSig>)options.GetConverter(typeof(ElectrumXVerboseScriptSig));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a verbose transaction input property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("A transaction input property name cannot be null.");
            if (!reader.Read())
            {
                throw new JsonException("Expected a verbose transaction input property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.VerboseTransactionInputResult.TransactionId:
                    transactionId = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionInputResult.OutputIndex:
                    outputIndex = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException("A verbose transaction input index and sequence must be integers.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionInputResult.ScriptSignature:
                    scriptSignature = reader.TokenType == JsonTokenType.Null
                        ? null
                        : scriptConverter.Read(ref reader, typeof(ElectrumXVerboseScriptSig), options);
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionInputResult.Sequence:
                    sequence = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetUInt64(out ulong value) => value,
                        _ => throw new JsonException("A verbose transaction input index and sequence must be integers.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionInputResult.Coinbase:
                    coinbase = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                default:
                    additionalProperties.Add(propertyName, JsonValueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options));
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The verbose transaction input is incomplete.");
        }

        return new ElectrumXVerboseTransactionInput(transactionId, outputIndex, scriptSignature, sequence, coinbase, additionalProperties);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXVerboseTransactionInput value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.VerboseTransactionInputResult.TransactionId, value.TransactionId);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionInputResult.OutputIndex);
        if (value.OutputIndex is {} outputIndex)
        {
            writer.WriteNumberValue(outputIndex);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionInputResult.ScriptSignature);
        if (value.ScriptSignature is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            var scriptConverter = (JsonConverter<ElectrumXVerboseScriptSig>)options.GetConverter(typeof(ElectrumXVerboseScriptSig));
            scriptConverter.Write(writer, value.ScriptSignature, options);
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionInputResult.Sequence);
        if (value.Sequence is {} sequence)
        {
            writer.WriteNumberValue(sequence);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.VerboseTransactionInputResult.Coinbase, value.Coinbase);
        foreach (var property in value.AdditionalProperties)
        {
            writer.WritePropertyName(property.Key);
            JsonValueConverter.Write(writer, property.Value, options);
        }

        writer.WriteEndObject();
    }
}
