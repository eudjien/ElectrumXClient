using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Mempool;

public sealed class MempoolResultJsonConverter : JsonConverter<ElectrumXMempool>
{
    public override bool HandleNull => true;

    public override ElectrumXMempool Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The mempool response must be an array.");
        }

        var entries = new List<ElectrumXMempoolEntry>();
        var entryConverter = (JsonConverter<ElectrumXMempoolEntry>)options.GetConverter(typeof(ElectrumXMempoolEntry));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            entries.Add(entryConverter.Read(ref reader, typeof(ElectrumXMempoolEntry), options)
                        ?? throw new JsonException("A mempool entry cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The mempool response must be an array.");
        }

        return new ElectrumXMempool(entries);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXMempool value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        var entryConverter = (JsonConverter<ElectrumXMempoolEntry>)options.GetConverter(typeof(ElectrumXMempoolEntry));
        foreach (var entry in value.Value)
        {
            entryConverter.Write(writer, entry, options);
        }

        writer.WriteEndArray();
    }
}
