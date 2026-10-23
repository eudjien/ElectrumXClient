using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class HistoryResultJsonConverter : JsonConverter<ElectrumXHistory>
{
    public override bool HandleNull => true;

    public override ElectrumXHistory Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The transaction history response must be an array.");
        }

        var entries = new List<ElectrumXHistoryEntry>();
        var entryConverter = (JsonConverter<ElectrumXHistoryEntry>)options.GetConverter(typeof(ElectrumXHistoryEntry));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            entries.Add(entryConverter.Read(ref reader, typeof(ElectrumXHistoryEntry), options)
                        ?? throw new JsonException("A transaction history entry cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The transaction history response must be an array.");
        }

        return new ElectrumXHistory(entries);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXHistory value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        var entryConverter = (JsonConverter<ElectrumXHistoryEntry>)options.GetConverter(typeof(ElectrumXHistoryEntry));
        foreach (var entry in value.Value)
        {
            entryConverter.Write(writer, entry, options);
        }

        writer.WriteEndArray();
    }
}
