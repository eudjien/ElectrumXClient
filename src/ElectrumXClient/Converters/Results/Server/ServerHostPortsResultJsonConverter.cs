using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class ServerHostPortsResultJsonConverter : JsonConverter<ElectrumXServerHostPorts>
{
    public override bool HandleNull => true;

    public override ElectrumXServerHostPorts Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        int? sslPort = null;
        int? tcpPort = null;
        int? webSocketPort = null;
        int? webSocketSecurePort = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a property name.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("Expected a property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.ServerHost.SslPort:
                    if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int ssl))
                    {
                        sslPort = ssl;
                    }
                    else if (reader.TokenType != JsonTokenType.Null)
                    {
                        throw new JsonException($"Property '{propertyName}' must be an integer or null.");
                    }

                    break;
                case ElectrumXJsonPropertyNames.ServerHost.TcpPort:
                    if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int tcp))
                    {
                        tcpPort = tcp;
                    }
                    else if (reader.TokenType != JsonTokenType.Null)
                    {
                        throw new JsonException($"Property '{propertyName}' must be an integer or null.");
                    }

                    break;
                case ElectrumXJsonPropertyNames.ServerHost.WebSocketPort:
                    if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int webSocket))
                    {
                        webSocketPort = webSocket;
                    }
                    else if (reader.TokenType != JsonTokenType.Null)
                    {
                        throw new JsonException($"Property '{propertyName}' must be an integer or null.");
                    }

                    break;
                case ElectrumXJsonPropertyNames.ServerHost.WebSocketSecurePort:
                    if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int webSocketSecure))
                    {
                        webSocketSecurePort = webSocketSecure;
                    }
                    else if (reader.TokenType != JsonTokenType.Null)
                    {
                        throw new JsonException($"Property '{propertyName}' must be an integer or null.");
                    }

                    break;
                default:
                    if (reader.TokenType is JsonTokenType.StartObject or JsonTokenType.StartArray)
                    {
                        int valueDepth = reader.CurrentDepth;
                        while (reader.Read() && reader.CurrentDepth > valueDepth)
                        {
                        }
                    }

                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Expected the end of a JSON object.");
        }

        return new ElectrumXServerHostPorts(sslPort, tcpPort, webSocketPort, webSocketSecurePort);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXServerHostPorts value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        if (value.SslPort is {} sslPort)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.ServerHost.SslPort, sslPort);
        }
        else
        {
            writer.WriteNull(ElectrumXJsonPropertyNames.ServerHost.SslPort);
        }

        if (value.TcpPort is {} tcpPort)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.ServerHost.TcpPort, tcpPort);
        }
        else
        {
            writer.WriteNull(ElectrumXJsonPropertyNames.ServerHost.TcpPort);
        }

        if (value.WebSocketPort is {} webSocketPort)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.ServerHost.WebSocketPort, webSocketPort);
        }
        else
        {
            writer.WriteNull(ElectrumXJsonPropertyNames.ServerHost.WebSocketPort);
        }

        if (value.WebSocketSecurePort is {} webSocketSecurePort)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.ServerHost.WebSocketSecurePort, webSocketSecurePort);
        }
        else
        {
            writer.WriteNull(ElectrumXJsonPropertyNames.ServerHost.WebSocketSecurePort);
        }

        writer.WriteEndObject();
    }
}
