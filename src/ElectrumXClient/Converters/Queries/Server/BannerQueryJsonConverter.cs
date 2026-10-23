using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Server;

public sealed class BannerQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXBannerQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXBannerQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();

        writer.WriteEndArray();
    }
}
