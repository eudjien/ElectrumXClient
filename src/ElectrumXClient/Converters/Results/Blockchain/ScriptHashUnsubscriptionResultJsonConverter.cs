using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class ScriptHashUnsubscriptionResultJsonConverter
    : JsonConverter<ElectrumXScriptHashUnsubscription>
{

    public override bool HandleNull => true;

    public override ElectrumXScriptHashUnsubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
        {
            throw new JsonException("The script-hash unsubscription response must be a boolean.");
        }

        return new ElectrumXScriptHashUnsubscription(reader.GetBoolean());
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXScriptHashUnsubscription value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteBooleanValue(value.Value);
    }
}
