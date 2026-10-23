using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results;

public sealed class ElectrumXJsonValueEnvelopeResultJsonConverter : JsonConverter<ElectrumXJsonValueResult>
{
    public override bool HandleNull => true;

    public override ElectrumXJsonValueResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
        var value = valueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options)
                    ?? throw new JsonException("The JSON result value was null.");
        return new ElectrumXJsonValueResult(value);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXJsonValueResult value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var valueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
        valueConverter.Write(writer, value.Value, options);
    }
}
