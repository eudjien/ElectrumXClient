using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class ListUnspentQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXListUnspentQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXListUnspentQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.ScriptHash);

        if (value.TokenFilter is not null)
        {
            writer.WriteStringValue(value.TokenFilter.Value switch
            {
                ElectrumXTokenFilter.IncludeTokens => ElectrumXJsonPropertyValues.TokenFilter.IncludeTokens,
                ElectrumXTokenFilter.ExcludeTokens => ElectrumXJsonPropertyValues.TokenFilter.ExcludeTokens,
                ElectrumXTokenFilter.TokensOnly => ElectrumXJsonPropertyValues.TokenFilter.TokensOnly,
                _ => throw new JsonException("The query token filter is not supported.")
            });
        }

        writer.WriteEndArray();
    }
}
