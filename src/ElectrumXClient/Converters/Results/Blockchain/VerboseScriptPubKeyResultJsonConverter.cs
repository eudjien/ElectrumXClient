using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class VerboseScriptPubKeyResultJsonConverter : JsonConverter<ElectrumXVerboseScriptPubKey>
{

    private static readonly ElectrumXJsonValueResultJsonConverter JsonValueConverter = new();
    public override bool HandleNull => true;

    public override ElectrumXVerboseScriptPubKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("A verbose script must be an object.");
        }

        string? assembly = null;
        string? hex = null;
        int? requiredSignatures = null;
        string? scriptType = null;
        IReadOnlyList<string>? addresses = null;
        var additionalProperties = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a verbose script property name.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("Expected a verbose script property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Assembly:
                    assembly = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Hex:
                    hex = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.RequiredSignatures:
                    requiredSignatures = reader.TokenType == JsonTokenType.Null
                        ? null
                        : reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int signatures)
                            ? signatures
                            : throw new JsonException($"Property '{propertyName}' must be an integer or null.");
                    break;
                case ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Type:
                    scriptType = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Addresses:
                    {
                        if (reader.TokenType == JsonTokenType.Null)
                        {
                            addresses = null;
                        }
                        else
                        {
                            if (reader.TokenType != JsonTokenType.StartArray)
                            {
                                throw new JsonException("Script addresses must be an array or null.");
                            }

                            var addressItems = new List<string>();
                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType != JsonTokenType.String)
                                {
                                    throw new JsonException("A script address must be a string.");
                                }

                                addressItems.Add(reader.GetString()
                                                 ?? throw new JsonException("A script address cannot be null."));
                            }

                            if (reader.TokenType != JsonTokenType.EndArray)
                            {
                                throw new JsonException("The script addresses array is incomplete.");
                            }

                            addresses = addressItems;
                        }
                        break;
                    }
                default:
                    string name = propertyName
                                  ?? throw new JsonException("A script property name cannot be null.");
                    additionalProperties.Add(name, JsonValueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options));
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The verbose script object is incomplete.");
        }

        return new ElectrumXVerboseScriptPubKey(assembly, hex, requiredSignatures, scriptType, addresses, additionalProperties);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXVerboseScriptPubKey value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Assembly, value.Assembly);
        writer.WriteString(ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Hex, value.Hex);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.RequiredSignatures);
        if (value.RequiredSignatures is {} requiredSignatures)
        {
            writer.WriteNumberValue(requiredSignatures);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Type, value.Type);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Addresses);
        if (value.Addresses is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();
            foreach (string address in value.Addresses)
            {
                writer.WriteStringValue(address);
            }

            writer.WriteEndArray();
        }

        foreach (var property in value.AdditionalProperties)
        {
            writer.WritePropertyName(property.Key);
            JsonValueConverter.Write(writer, property.Value, options);
        }

        writer.WriteEndObject();
    }
}
