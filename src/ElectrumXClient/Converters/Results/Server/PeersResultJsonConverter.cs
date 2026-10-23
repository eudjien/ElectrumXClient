using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class PeersResultJsonConverter : JsonConverter<ElectrumXPeers>
{
    public override bool HandleNull => true;

    public override ElectrumXPeers Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The peer response must be an array.");
        }

        var peers = new List<ElectrumXPeer>();
        var peerConverter = (JsonConverter<ElectrumXPeer>)options.GetConverter(typeof(ElectrumXPeer));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            peers.Add(peerConverter.Read(ref reader, typeof(ElectrumXPeer), options)
                      ?? throw new JsonException("A peer entry cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The peer response must be an array.");
        }

        return new ElectrumXPeers(peers);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXPeers value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        var peerConverter = (JsonConverter<ElectrumXPeer>)options.GetConverter(typeof(ElectrumXPeer));
        foreach (var peer in value.Value)
        {
            peerConverter.Write(writer, peer, options);
        }

        writer.WriteEndArray();
    }
}
