using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class ServerFeaturesResultJsonConverter : JsonConverter<ElectrumXServerFeatures>
{
    public override bool HandleNull => true;

    public override ElectrumXServerFeatures Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Server features must be a JSON object.");
        }

        Dictionary<string, ElectrumXServerHostPorts>? hosts = null;
        string? genesisHash = null;
        string? hashFunction = null;
        string? serverVersion = null;
        string? protocolMinimum = null;
        string? protocolMaximum = null;
        int? pruning = null;
        var additionalProperties = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);
        var hostConverter = (JsonConverter<ElectrumXServerHostPorts>)options.GetConverter(typeof(ElectrumXServerHostPorts));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a server feature property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("A server feature property name cannot be null.");
            if (!reader.Read())
            {
                throw new JsonException("Expected a server feature property value.");
            }

            if (propertyName == ElectrumXJsonPropertyNames.ServerFeaturesResult.Hosts)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    throw new JsonException("The server feature hosts property cannot be null.");
                }

                hosts = new Dictionary<string, ElectrumXServerHostPorts>();
                if (reader.TokenType != JsonTokenType.StartObject)
                {
                    throw new JsonException("Server hosts must be a JSON object.");
                }

                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException("Expected a server hostname.");
                    }

                    string hostName = reader.GetString()
                                      ?? throw new JsonException("A server hostname cannot be null.");
                    if (!reader.Read())
                    {
                        throw new JsonException($"Server host '{hostName}' has no ports value.");
                    }

                    hosts.Add(
                        hostName,
                        hostConverter.Read(ref reader, typeof(ElectrumXServerHostPorts), options)
                        ?? throw new JsonException($"Server host '{hostName}' has null ports."));
                }

                if (reader.TokenType != JsonTokenType.EndObject)
                {
                    throw new JsonException("The server hosts object is incomplete.");
                }
            }
            else if (propertyName == ElectrumXJsonPropertyNames.ServerFeaturesResult.GenesisHash)
            {
                genesisHash = reader.TokenType == JsonTokenType.String
                    ? reader.GetString()
                      ?? throw new JsonException($"The server feature '{propertyName}' must be a string.")
                    : throw new JsonException($"The server feature '{propertyName}' must be a string.");
            }
            else if (propertyName == ElectrumXJsonPropertyNames.ServerFeaturesResult.HashFunction)
            {
                hashFunction = reader.TokenType == JsonTokenType.String
                    ? reader.GetString()
                      ?? throw new JsonException($"The server feature '{propertyName}' must be a string.")
                    : throw new JsonException($"The server feature '{propertyName}' must be a string.");
            }
            else if (propertyName == ElectrumXJsonPropertyNames.ServerFeaturesResult.ServerVersion)
            {
                serverVersion = reader.TokenType == JsonTokenType.String
                    ? reader.GetString()
                      ?? throw new JsonException($"The server feature '{propertyName}' must be a string.")
                    : throw new JsonException($"The server feature '{propertyName}' must be a string.");
            }
            else if (propertyName == ElectrumXJsonPropertyNames.ServerFeaturesResult.ProtocolMinimum)
            {
                protocolMinimum = reader.TokenType == JsonTokenType.String
                    ? reader.GetString()
                      ?? throw new JsonException($"The server feature '{propertyName}' must be a string.")
                    : throw new JsonException($"The server feature '{propertyName}' must be a string.");
            }
            else if (propertyName == ElectrumXJsonPropertyNames.ServerFeaturesResult.ProtocolMaximum)
            {
                protocolMaximum = reader.TokenType == JsonTokenType.String
                    ? reader.GetString()
                      ?? throw new JsonException($"The server feature '{propertyName}' must be a string.")
                    : throw new JsonException($"The server feature '{propertyName}' must be a string.");
            }
            else if (propertyName == ElectrumXJsonPropertyNames.ServerFeaturesResult.Pruning)
            {
                pruning = reader.TokenType switch
                {
                    JsonTokenType.Null => null,
                    JsonTokenType.Number when reader.TryGetInt32(out int value) => value,
                    _ => throw new JsonException("The server pruning limit must be an integer or null.")
                };
            }
            else
            {
                string name = propertyName;
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
                                    _ => throw new JsonException("Mismatched additional server feature container.")
                                };
                                break;
                            }
                        case JsonTokenType.PropertyName:
                            if (containers.Count == 0 ||
                                containers.Peek() is not Dictionary<string, ElectrumXJsonValue>)
                            {
                                throw new JsonException("Expected an additional server feature property name.");
                            }

                            string nestedPropertyName = reader.GetString()
                                                        ?? throw new JsonException(
                                                            "An additional server feature property name cannot be null.");
                            pendingNames.Pop();
                            pendingNames.Push(nestedPropertyName);
                            if (!reader.Read())
                            {
                                throw new JsonException("An additional server feature property value is missing.");
                            }

                            continue;
                        case JsonTokenType.String:
                            completedValue = new ElectrumXJsonValue.StringValue(
                                reader.GetString()
                                ?? throw new JsonException("A JSON string value cannot be null."));
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
                            throw new JsonException($"Unsupported JSON value kind '{reader.TokenType}'.");
                    }

                    if (completedValue is not null)
                    {
                        if (containers.Count == 0)
                        {
                            additionalValue = completedValue;
                        }
                        else if (containers.Peek() is List<ElectrumXJsonValue> items)
                        {
                            items.Add(completedValue);
                        }
                        else if (containers.Peek() is Dictionary<string, ElectrumXJsonValue> properties)
                        {
                            string childName = pendingNames.Pop()
                                               ?? throw new JsonException(
                                                   "An additional server feature property name is missing.");
                            properties.Add(childName, completedValue);
                            pendingNames.Push(null);
                        }
                    }

                    if (additionalValue is null && !reader.Read())
                    {
                        throw new JsonException("An additional server feature value is incomplete.");
                    }
                }

                additionalProperties.Add(
                    name,
                    additionalValue);
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The server features object is incomplete.");
        }

        return new ElectrumXServerFeatures(
            hosts ?? new Dictionary<string, ElectrumXServerHostPorts>(StringComparer.Ordinal),
            genesisHash ?? throw new JsonException("The required server feature 'genesis_hash' is missing."),
            hashFunction ?? throw new JsonException("The required server feature 'hash_function' is missing."),
            serverVersion ?? throw new JsonException("The required server feature 'server_version' is missing."),
            protocolMinimum ?? throw new JsonException("The required server feature 'protocol_min' is missing."),
            protocolMaximum ?? throw new JsonException("The required server feature 'protocol_max' is missing."),
            pruning,
            additionalProperties);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXServerFeatures value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WritePropertyName(ElectrumXJsonPropertyNames.ServerFeaturesResult.Hosts);
        writer.WriteStartObject();
        var hostConverter = (JsonConverter<ElectrumXServerHostPorts>)options.GetConverter(typeof(ElectrumXServerHostPorts));
        foreach ((string hostName, var ports) in value.Hosts)
        {
            writer.WritePropertyName(hostName);
            hostConverter.Write(writer, ports, options);
        }

        writer.WriteEndObject();
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.GenesisHash, value.GenesisHash);
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.HashFunction, value.HashFunction);
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.ServerVersion, value.ServerVersion);
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.ProtocolMinimum, value.ProtocolMinimum);
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.ProtocolMaximum, value.ProtocolMaximum);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.ServerFeaturesResult.Pruning);
        if (value.Pruning is {} pruning)
        {
            writer.WriteNumberValue(pruning);
        }
        else
        {
            writer.WriteNullValue();
        }

        foreach ((string name, var propertyValue) in value.AdditionalProperties)
        {
            writer.WritePropertyName(name);
            WriteJsonValue(writer, propertyValue);
        }

        writer.WriteEndObject();
    }

    private static void WriteJsonValue(Utf8JsonWriter writer, ElectrumXJsonValue value)
    {
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
                    WriteJsonValue(writer, item);
                }

                writer.WriteEndArray();
                break;
            case ElectrumXJsonValue.ObjectValue objectValue:
                writer.WriteStartObject();
                foreach ((string name, var propertyValue) in objectValue.Properties)
                {
                    writer.WritePropertyName(name);
                    WriteJsonValue(writer, propertyValue);
                }

                writer.WriteEndObject();
                break;
            default:
                throw new JsonException($"Unsupported ElectrumX JSON value type '{value.GetType().Name}'.");
        }
    }
}
