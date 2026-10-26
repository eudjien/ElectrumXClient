using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class VerboseTransactionResultJsonConverter : JsonConverter<ElectrumXVerboseTransaction>
{
    public override bool HandleNull => true;

    public override ElectrumXVerboseTransaction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("A verbose transaction must be an object.");
        }

        string? blockHash = null;
        long? blockTime = null;
        long? confirmations = null;
        string? hash = null;
        string? hex = null;
        long? lockTime = null;
        long? size = null;
        long? time = null;
        string? transactionId = null;
        long? version = null;
        IReadOnlyList<ElectrumXVerboseTransactionInput>? inputs = null;
        IReadOnlyList<ElectrumXVerboseTransactionOutput>? outputs = null;
        var additionalProperties = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);
        var inputConverter = (JsonConverter<ElectrumXVerboseTransactionInput>)options.GetConverter(typeof(ElectrumXVerboseTransactionInput));
        var outputConverter = (JsonConverter<ElectrumXVerboseTransactionOutput>)options.GetConverter(typeof(ElectrumXVerboseTransactionOutput));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a verbose transaction property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("A verbose transaction property name cannot be null.");
            if (!reader.Read())
            {
                throw new JsonException("Expected a verbose transaction property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.BlockHash:
                    blockHash = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.BlockTime:
                    blockTime = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.Confirmations:
                    confirmations = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.Hash:
                    hash = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.Hex:
                    hex = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.LockTime:
                    lockTime = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.Size:
                    size = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.Time:
                    time = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.TransactionId:
                    transactionId = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.Version:
                    version = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.Inputs:
                    {
                        if (reader.TokenType == JsonTokenType.Null)
                        {
                            inputs = null;
                        }
                        else
                        {
                            if (reader.TokenType != JsonTokenType.StartArray)
                            {
                                throw new JsonException($"Property '{propertyName}' must be an array or null.");
                            }

                            var inputItems = new List<ElectrumXVerboseTransactionInput>();
                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType == JsonTokenType.Null)
                                {
                                    throw new JsonException($"An item in '{propertyName}' cannot be null.");
                                }

                                inputItems.Add(inputConverter.Read(
                                                   ref reader,
                                                   typeof(ElectrumXVerboseTransactionInput),
                                                   options)
                                               ?? throw new JsonException($"An item in '{propertyName}' cannot be null."));
                            }

                            inputs = reader.TokenType == JsonTokenType.EndArray
                                ? inputItems
                                : throw new JsonException($"The array property '{propertyName}' is incomplete.");
                        }
                        break;
                    }
                case ElectrumXJsonPropertyNames.VerboseTransactionResult.Outputs:
                    {
                        if (reader.TokenType == JsonTokenType.Null)
                        {
                            outputs = null;
                        }
                        else
                        {
                            if (reader.TokenType != JsonTokenType.StartArray)
                            {
                                throw new JsonException($"Property '{propertyName}' must be an array or null.");
                            }

                            var outputItems = new List<ElectrumXVerboseTransactionOutput>();
                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType == JsonTokenType.Null)
                                {
                                    throw new JsonException($"An item in '{propertyName}' cannot be null.");
                                }

                                outputItems.Add(outputConverter.Read(
                                                    ref reader,
                                                    typeof(ElectrumXVerboseTransactionOutput),
                                                    options)
                                                ?? throw new JsonException($"An item in '{propertyName}' cannot be null."));
                            }

                            outputs = reader.TokenType == JsonTokenType.EndArray
                                ? outputItems
                                : throw new JsonException($"The array property '{propertyName}' is incomplete.");
                        }
                        break;
                    }
                default:
                    ElectrumXJsonValue? additionalValue = null;
                    var containers = new Stack<object>();
                    var pendingNames = new Stack<string?>();

                    while (additionalValue is null)
                    {
                        ElectrumXJsonValue? completedValue = null;

                        switch (reader.TokenType)
                        {
                            case JsonTokenType.StartArray:
                                containers.Push(new List<ElectrumXJsonValue>());
                                pendingNames.Push(null);
                                break;
                            case JsonTokenType.StartObject:
                                containers.Push(new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal));
                                pendingNames.Push(null);
                                break;
                            case JsonTokenType.EndArray:
                            case JsonTokenType.EndObject:
                                {
                                    object container = containers.Pop();
                                    pendingNames.Pop();
                                    completedValue = (container, reader.TokenType) switch
                                    {
                                        (List<ElectrumXJsonValue> items, JsonTokenType.EndArray) =>
                                            new ElectrumXJsonValue.ArrayValue(items),
                                        (Dictionary<string, ElectrumXJsonValue> properties, JsonTokenType.EndObject) =>
                                            new ElectrumXJsonValue.ObjectValue(properties),
                                        _ => throw new JsonException("Mismatched verbose transaction extension container.")
                                    };
                                    break;
                                }
                            case JsonTokenType.PropertyName:
                                if (containers.Count == 0 ||
                                    containers.Peek() is not Dictionary<string, ElectrumXJsonValue>)
                                {
                                    throw new JsonException("Expected a verbose transaction extension property name.");
                                }

                                string nestedPropertyName = reader.GetString()
                                                            ?? throw new JsonException(
                                                                "A verbose transaction extension property name cannot be null.");
                                pendingNames.Pop();
                                pendingNames.Push(nestedPropertyName);
                                if (!reader.Read())
                                {
                                    throw new JsonException("A verbose transaction extension property value is missing.");
                                }

                                continue;
                            case JsonTokenType.String:
                                completedValue = new ElectrumXJsonValue.StringValue(
                                    reader.GetString() ?? throw new JsonException("A JSON string value cannot be null."));
                                break;
                            case JsonTokenType.Number:
                                string rawNumber = reader.HasValueSequence
                                    ? Encoding.UTF8.GetString(reader.ValueSequence.ToArray())
                                    : Encoding.UTF8.GetString(reader.ValueSpan);
                                completedValue = ElectrumXJsonValue.NumberValue.FromRawJson(rawNumber);
                                break;
                            case JsonTokenType.True:
                                completedValue = new ElectrumXJsonValue.BooleanValue(true);
                                break;
                            case JsonTokenType.False:
                                completedValue = new ElectrumXJsonValue.BooleanValue(false);
                                break;
                            case JsonTokenType.Null:
                                completedValue = ElectrumXJsonValue.NullValue.Instance;
                                break;
                            default:
                                throw new JsonException($"Unsupported verbose transaction token '{reader.TokenType}'.");
                        }

                        if (completedValue is not null)
                        {
                            if (containers.Count == 0)
                            {
                                additionalValue = completedValue;
                            }
                            else
                            {
                                switch (containers.Peek())
                                {
                                    case List<ElectrumXJsonValue> items:
                                        items.Add(completedValue);
                                        break;
                                    case Dictionary<string, ElectrumXJsonValue> properties:
                                        string nestedName = pendingNames.Pop()
                                                            ?? throw new JsonException("A verbose transaction extension property name is missing.");
                                        properties.Add(nestedName, completedValue);
                                        pendingNames.Push(null);
                                        break;
                                    default:
                                        throw new JsonException("Unsupported verbose transaction extension container.");
                                }
                            }
                        }

                        if (additionalValue is null && !reader.Read())
                        {
                            throw new JsonException("The verbose transaction extension value is incomplete.");
                        }
                    }

                    additionalProperties.Add(propertyName, additionalValue);
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The verbose transaction object is incomplete.");
        }

        return new ElectrumXVerboseTransaction(
            blockHash, blockTime, confirmations, hash, hex, lockTime, size, time, transactionId, version, inputs, outputs,
            additionalProperties);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXVerboseTransaction value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.VerboseTransactionResult.BlockHash, value.BlockHash);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionResult.BlockTime);
        if (value.BlockTime is {} blockTime)
        {
            writer.WriteNumberValue(blockTime);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionResult.Confirmations);
        if (value.Confirmations is {} confirmations)
        {
            writer.WriteNumberValue(confirmations);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.VerboseTransactionResult.Hash, value.Hash);
        writer.WriteString(ElectrumXJsonPropertyNames.VerboseTransactionResult.Hex, value.Hex);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionResult.LockTime);
        if (value.LockTime is {} lockTime)
        {
            writer.WriteNumberValue(lockTime);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionResult.Size);
        if (value.Size is {} size)
        {
            writer.WriteNumberValue(size);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionResult.Time);
        if (value.Time is {} time)
        {
            writer.WriteNumberValue(time);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.VerboseTransactionResult.TransactionId, value.TransactionId);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionResult.Version);
        if (value.Version is {} version)
        {
            writer.WriteNumberValue(version);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionResult.Inputs);
        if (value.Inputs is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();
            var inputConverter = (JsonConverter<ElectrumXVerboseTransactionInput>)options.GetConverter(typeof(ElectrumXVerboseTransactionInput));
            foreach (var input in value.Inputs)
            {
                inputConverter.Write(writer, input, options);
            }

            writer.WriteEndArray();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionResult.Outputs);
        if (value.Outputs is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();
            var outputConverter = (JsonConverter<ElectrumXVerboseTransactionOutput>)options.GetConverter(typeof(ElectrumXVerboseTransactionOutput));
            foreach (var output in value.Outputs)
            {
                outputConverter.Write(writer, output, options);
            }

            writer.WriteEndArray();
        }

        var jsonValueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
        foreach ((string name, var propertyValue) in value.AdditionalProperties)
        {
            if (name is ElectrumXJsonPropertyNames.VerboseTransactionResult.BlockHash or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.BlockTime or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.Confirmations or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.Hash or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.Hex or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.LockTime or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.Size or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.Time or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.TransactionId or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.Version or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.Inputs or
                ElectrumXJsonPropertyNames.VerboseTransactionResult.Outputs)
            {
                continue;
            }

            writer.WritePropertyName(name);
            jsonValueConverter.Write(writer, propertyValue, options);
        }

        writer.WriteEndObject();
    }
}
