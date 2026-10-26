using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class HistoryListResultJsonConverter : JsonConverter<ElectrumXHistoryList>
{
    public override ElectrumXHistoryList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The result must be a transaction-history array.");
        }

        var values = new List<ElectrumXHistoryEntry>();
        var converter = (JsonConverter<ElectrumXHistoryEntry>)options.GetConverter(typeof(ElectrumXHistoryEntry));
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            values.Add(converter.Read(ref reader, typeof(ElectrumXHistoryEntry), options)
                       ?? throw new JsonException("A transaction-history item cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The result array is incomplete.");
        }

        return new ElectrumXHistoryList(values);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXHistoryList value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        var converter = (JsonConverter<ElectrumXHistoryEntry>)options.GetConverter(typeof(ElectrumXHistoryEntry));
        foreach (var item in value.Value)
        {
            converter.Write(writer, item, options);
        }

        writer.WriteEndArray();
    }
}
