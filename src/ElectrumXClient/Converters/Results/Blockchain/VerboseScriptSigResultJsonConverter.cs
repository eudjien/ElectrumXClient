using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class VerboseScriptSigResultJsonConverter : JsonConverter<ElectrumXVerboseScriptSig>
{

    private static readonly ElectrumXJsonValueJsonConverter JsonValueConverter = new();
    public override bool HandleNull => true;

    public override ElectrumXVerboseScriptSig Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("A verbose scriptSig must be an object.");
        }

        string? assembly = null;
        string? hex = null;
        var additionalProperties = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a verbose scriptSig property name.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("Expected a verbose scriptSig property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.VerboseScriptSigResult.Assembly:
                    assembly = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseScriptSigResult.Hex:
                    hex = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                default:
                    string name = propertyName
                                  ?? throw new JsonException("A scriptSig property name cannot be null.");
                    additionalProperties.Add(name, JsonValueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options));
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The verbose scriptSig object is incomplete.");
        }

        return new ElectrumXVerboseScriptSig(assembly, hex, additionalProperties);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXVerboseScriptSig value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.VerboseScriptSigResult.Assembly, value.Assembly);
        writer.WriteString(ElectrumXJsonPropertyNames.VerboseScriptSigResult.Hex, value.Hex);
        foreach (var property in value.AdditionalProperties)
        {
            writer.WritePropertyName(property.Key);
            JsonValueConverter.Write(writer, property.Value, options);
        }

        writer.WriteEndObject();
    }
}
