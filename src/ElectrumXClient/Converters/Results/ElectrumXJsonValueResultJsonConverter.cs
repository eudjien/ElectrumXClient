using NorthGomel.ElectrumXClient.Abstractions.Models;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results;

internal sealed class ElectrumXJsonValueResultJsonConverter : JsonConverter<ElectrumXJsonValue>
{
    public override bool HandleNull => true;

    public override ElectrumXJsonValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                return new ElectrumXJsonValue.StringValue(
                    reader.GetString() ?? throw new JsonException("A JSON string value cannot be null."));
            case JsonTokenType.Number:
                {
                    string rawNumber = reader.HasValueSequence
                        ? Encoding.UTF8.GetString(reader.ValueSequence.ToArray())
                        : Encoding.UTF8.GetString(reader.ValueSpan);
                    return ElectrumXJsonValue.NumberValue.FromRawJson(rawNumber);
                }
            case JsonTokenType.True:
                return new ElectrumXJsonValue.BooleanValue(true);
            case JsonTokenType.False:
                return new ElectrumXJsonValue.BooleanValue(false);
            case JsonTokenType.Null:
                return ElectrumXJsonValue.NullValue.Instance;
            case JsonTokenType.StartArray:
                {
                    var items = new List<ElectrumXJsonValue>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        items.Add(Read(ref reader, typeof(ElectrumXJsonValue), options));
                    }

                    if (reader.TokenType != JsonTokenType.EndArray)
                    {
                        throw new JsonException("The JSON array value is incomplete.");
                    }

                    return new ElectrumXJsonValue.ArrayValue(items);
                }
            case JsonTokenType.StartObject:
                {
                    var properties = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType != JsonTokenType.PropertyName)
                        {
                            throw new JsonException("Expected a JSON object property name.");
                        }

                        string name = reader.GetString()
                                      ?? throw new JsonException("A JSON object property name cannot be null.");
                        if (!reader.Read())
                        {
                            throw new JsonException("A JSON object property value is missing.");
                        }

                        properties.Add(name, Read(ref reader, typeof(ElectrumXJsonValue), options));
                    }

                    if (reader.TokenType != JsonTokenType.EndObject)
                    {
                        throw new JsonException("The JSON object value is incomplete.");
                    }

                    return new ElectrumXJsonValue.ObjectValue(properties);
                }
            default:
                throw new JsonException($"Unsupported JSON token '{reader.TokenType}'.");
        }
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXJsonValue value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        switch (value)
        {
            case ElectrumXJsonValue.StringValue stringValue:
                writer.WriteStringValue(stringValue.Value);
                break;
            case ElectrumXJsonValue.NumberValue numberValue:
                writer.WriteRawValue(numberValue.RawJson);
                break;
            case ElectrumXJsonValue.BooleanValue booleanValue:
                writer.WriteBooleanValue(booleanValue.Value);
                break;
            case ElectrumXJsonValue.NullValue:
                writer.WriteNullValue();
                break;
            case ElectrumXJsonValue.ArrayValue arrayValue:
                writer.WriteStartArray();
                foreach (var item in arrayValue.Items)
                {
                    Write(writer, item, options);
                }

                writer.WriteEndArray();
                break;
            case ElectrumXJsonValue.ObjectValue objectValue:
                writer.WriteStartObject();
                foreach (var property in objectValue.Properties)
                {
                    writer.WritePropertyName(property.Key);
                    Write(writer, property.Value, options);
                }

                writer.WriteEndObject();
                break;
            default:
                throw new JsonException($"Unsupported ElectrumX JSON value type '{value.GetType().Name}'.");
        }
    }
}
