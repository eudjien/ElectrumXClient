using NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.MasterNode;

public sealed class MasterNodesResultJsonConverter : JsonConverter<ElectrumXMasterNodes>
{
    public override bool HandleNull => true;

    public override ElectrumXMasterNodes Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The master-node response must be an array.");
        }

        var nodes = new List<ElectrumXMasterNode>();
        var nodeConverter = (JsonConverter<ElectrumXMasterNode>)options.GetConverter(typeof(ElectrumXMasterNode));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            nodes.Add(nodeConverter.Read(ref reader, typeof(ElectrumXMasterNode), options)
                      ?? throw new JsonException("A master-node entry cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The master-node response must be an array.");
        }

        return new ElectrumXMasterNodes(nodes);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXMasterNodes value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        var nodeConverter = (JsonConverter<ElectrumXMasterNode>)options.GetConverter(typeof(ElectrumXMasterNode));
        foreach (var node in value.Value)
        {
            nodeConverter.Write(writer, node, options);
        }

        writer.WriteEndArray();
    }
}
