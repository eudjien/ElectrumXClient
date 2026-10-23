using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Server;

public sealed class AddPeerQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddPeerQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddPeerQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        var features = new ElectrumXServerFeatures(
            value.Hosts,
            value.GenesisHash,
            value.HashFunction,
            value.ServerVersion,
            value.ProtocolMinimum,
            value.ProtocolMaximum,
            value.Pruning,
            value.AdditionalProperties);

        writer.WriteStartArray();
        writer.WriteStartObject();

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ServerFeaturesResult.Hosts);
        writer.WriteStartObject();
        foreach (var host in features.Hosts)
        {
            writer.WritePropertyName(host.Key);
            writer.WriteStartObject();
            if (host.Value.SslPort is {} sslPort)
            {
                writer.WriteNumber(ElectrumXJsonPropertyNames.ServerHost.SslPort, sslPort);
            }
            else
            {
                writer.WriteNull(ElectrumXJsonPropertyNames.ServerHost.SslPort);
            }

            if (host.Value.TcpPort is {} tcpPort)
            {
                writer.WriteNumber(ElectrumXJsonPropertyNames.ServerHost.TcpPort, tcpPort);
            }
            else
            {
                writer.WriteNull(ElectrumXJsonPropertyNames.ServerHost.TcpPort);
            }

            if (host.Value.WebSocketPort is {} webSocketPort)
            {
                writer.WriteNumber(ElectrumXJsonPropertyNames.ServerHost.WebSocketPort, webSocketPort);
            }
            else
            {
                writer.WriteNull(ElectrumXJsonPropertyNames.ServerHost.WebSocketPort);
            }

            if (host.Value.WebSocketSecurePort is {} webSocketSecurePort)
            {
                writer.WriteNumber(ElectrumXJsonPropertyNames.ServerHost.WebSocketSecurePort, webSocketSecurePort);
            }
            else
            {
                writer.WriteNull(ElectrumXJsonPropertyNames.ServerHost.WebSocketSecurePort);
            }

            writer.WriteEndObject();
        }

        writer.WriteEndObject();
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.GenesisHash, features.GenesisHash);
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.HashFunction, features.HashFunction);
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.ServerVersion, features.ServerVersion);
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.ProtocolMinimum, features.ProtocolMinimum);
        writer.WriteString(ElectrumXJsonPropertyNames.ServerFeaturesResult.ProtocolMaximum, features.ProtocolMaximum);
        if (features.Pruning is {} pruning)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.ServerFeaturesResult.Pruning, pruning);
        }
        else
        {
            writer.WriteNull(ElectrumXJsonPropertyNames.ServerFeaturesResult.Pruning);
        }

        foreach (var property in features.AdditionalProperties)
        {
            writer.WritePropertyName(property.Key);
            WriteJsonValue(writer, property.Value);
        }

        writer.WriteEndObject();

        writer.WriteEndArray();
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
                foreach (var property in objectValue.Properties)
                {
                    writer.WritePropertyName(property.Key);
                    WriteJsonValue(writer, property.Value);
                }

                writer.WriteEndObject();
                break;
            default:
                throw new JsonException($"Unsupported ElectrumX JSON value type '{value.GetType().Name}'.");
        }
    }
}
