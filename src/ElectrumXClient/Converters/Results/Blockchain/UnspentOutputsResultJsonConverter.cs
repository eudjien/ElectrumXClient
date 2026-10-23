using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class UnspentOutputsResultJsonConverter : JsonConverter<ElectrumXUnspentOutputs>
{
    public override bool HandleNull => true;

    public override ElectrumXUnspentOutputs Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The unspent outputs response must be an array.");
        }

        var outputs = new List<ElectrumXUnspentOutput>();
        var outputConverter = (JsonConverter<ElectrumXUnspentOutput>)options.GetConverter(typeof(ElectrumXUnspentOutput));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            outputs.Add(outputConverter.Read(ref reader, typeof(ElectrumXUnspentOutput), options)
                        ?? throw new JsonException("An unspent output cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The unspent outputs response must be an array.");
        }

        return new ElectrumXUnspentOutputs(outputs);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXUnspentOutputs value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        var outputConverter = (JsonConverter<ElectrumXUnspentOutput>)options.GetConverter(typeof(ElectrumXUnspentOutput));
        foreach (var output in value.Value)
        {
            outputConverter.Write(writer, output, options);
        }

        writer.WriteEndArray();
    }
}
