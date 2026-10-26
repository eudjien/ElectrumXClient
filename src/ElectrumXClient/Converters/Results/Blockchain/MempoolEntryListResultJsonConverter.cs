using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class MempoolEntryListResultJsonConverter : JsonConverter<ElectrumXMempoolEntryList>
{
    public override ElectrumXMempoolEntryList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The result must be a mempool-entry array.");
        }

        var values = new List<ElectrumXMempoolEntry>();
        var converter = (JsonConverter<ElectrumXMempoolEntry>)options.GetConverter(typeof(ElectrumXMempoolEntry));
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            values.Add(converter.Read(ref reader, typeof(ElectrumXMempoolEntry), options)
                       ?? throw new JsonException("A mempool entry cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The result array is incomplete.");
        }

        return new ElectrumXMempoolEntryList(values);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXMempoolEntryList value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        var converter = (JsonConverter<ElectrumXMempoolEntry>)options.GetConverter(typeof(ElectrumXMempoolEntry));
        foreach (var item in value.Value)
        {
            converter.Write(writer, item, options);
        }

        writer.WriteEndArray();
    }
}
