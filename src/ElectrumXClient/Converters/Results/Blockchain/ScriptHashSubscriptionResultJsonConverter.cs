using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class ScriptHashSubscriptionResultJsonConverter
    : JsonConverter<ElectrumXScriptHashSubscription>
{

    public override bool HandleNull => true;

    public override ElectrumXScriptHashSubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new ElectrumXScriptHashSubscription(null);
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The script-hash subscription response must be a string or null.");
        }

        return new ElectrumXScriptHashSubscription(reader.GetString());
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXScriptHashSubscription value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value);
    }
}
