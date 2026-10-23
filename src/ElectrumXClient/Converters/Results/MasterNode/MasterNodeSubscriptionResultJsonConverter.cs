using NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.MasterNode;

public sealed class MasterNodeSubscriptionResultJsonConverter
    : JsonConverter<ElectrumXMasterNodeSubscription>
{

    public override bool HandleNull => true;

    public override ElectrumXMasterNodeSubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return new ElectrumXMasterNodeSubscription(null);
        }

        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The master-node subscription response must be a string or null.");
        }

        return new ElectrumXMasterNodeSubscription(reader.GetString());
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXMasterNodeSubscription value,
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
