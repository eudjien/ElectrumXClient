using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class VerboseTransactionOutputResultJsonConverter : JsonConverter<ElectrumXVerboseTransactionOutput>
{

    private static readonly ElectrumXJsonValueResultJsonConverter JsonValueConverter = new();
    public override bool HandleNull => true;

    public override ElectrumXVerboseTransactionOutput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("A verbose transaction output must be an object.");
        }

        long? index = null;
        ElectrumXVerboseScriptPubKey? scriptPubKey = null;
        decimal? outputValue = null;
        var additionalProperties = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);
        var scriptConverter = (JsonConverter<ElectrumXVerboseScriptPubKey>)options.GetConverter(typeof(ElectrumXVerboseScriptPubKey));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a verbose transaction output property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("A transaction output property name cannot be null.");
            if (!reader.Read())
            {
                throw new JsonException("Expected a verbose transaction output property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.Index:
                    index = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException("The verbose output index must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.ScriptPubKey:
                    scriptPubKey = reader.TokenType == JsonTokenType.Null
                        ? null
                        : scriptConverter.Read(ref reader, typeof(ElectrumXVerboseScriptPubKey), options);
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.Value:
                    outputValue = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetDecimal(out decimal value) => value,
                        _ => throw new JsonException("The verbose output value must be a number or null.")
                    };
                    break;
                default:
                    additionalProperties.Add(propertyName, JsonValueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options));
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The verbose transaction output is incomplete.");
        }

        return new ElectrumXVerboseTransactionOutput(index, scriptPubKey, outputValue, additionalProperties);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXVerboseTransactionOutput value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.Index);
        if (value.Index is {} index)
        {
            writer.WriteNumberValue(index);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.ScriptPubKey);
        if (value.ScriptPubKey is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            var scriptConverter = (JsonConverter<ElectrumXVerboseScriptPubKey>)options.GetConverter(typeof(ElectrumXVerboseScriptPubKey));
            scriptConverter.Write(writer, value.ScriptPubKey, options);
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.Value);
        if (value.Value is {} outputValue)
        {
            writer.WriteNumberValue(outputValue);
        }
        else
        {
            writer.WriteNullValue();
        }

        foreach (var property in value.AdditionalProperties)
        {
            writer.WritePropertyName(property.Key);
            JsonValueConverter.Write(writer, property.Value, options);
        }

        writer.WriteEndObject();
    }
}
