using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Server;

public sealed class DaemonPassthroughQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXDaemonPassthroughQuery>
{
    public override bool HandleNull => true;

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXDaemonPassthroughQuery value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.DaemonPassthroughQuery.Method, value.Method);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.DaemonPassthroughQuery.Parameters);
        writer.WriteStartArray();
        var parameterConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
        foreach (var parameter in value.Parameters)
        {
            parameterConverter.Write(writer, parameter, options);
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }
}
