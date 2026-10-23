using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class PeerResultJsonConverter : JsonConverter<ElectrumXPeer>
{
    public override bool HandleNull => true;

    public override ElectrumXPeer Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray ||
            !reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("A peer entry must begin with a string address.");
        }

        string ipAddress = reader.GetString() ?? throw new JsonException("Peer address cannot be null.");

        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("A peer entry must contain a string host name.");
        }

        string hostName = reader.GetString() ?? throw new JsonException("Peer host name cannot be null.");

        if (!reader.Read() || reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Peer features must be an array.");
        }

        var features = new List<string>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Peer features must contain only strings.");
            }

            features.Add(reader.GetString() ?? throw new JsonException("A peer feature cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray ||
            !reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("A peer entry must contain exactly three elements.");
        }

        return new ElectrumXPeer(ipAddress, hostName, features);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXPeer value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.IpAddress);
        writer.WriteStringValue(value.HostName);
        writer.WriteStartArray();
        foreach (string feature in value.Features)
        {
            writer.WriteStringValue(feature);
        }

        writer.WriteEndArray();
        writer.WriteEndArray();
    }
}
