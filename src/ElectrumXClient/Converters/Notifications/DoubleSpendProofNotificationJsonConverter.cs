using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Notifications;

public sealed class DoubleSpendProofNotificationJsonConverter : JsonConverter<ElectrumXDoubleSpendProofNotification>
{
    public override ElectrumXDoubleSpendProofNotification Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray || !reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Double-spend-proof notification parameters must begin with a string transaction hash.");
        }

        string transactionHash = reader.GetString()
                                 ?? throw new JsonException("The double-spend-proof transaction hash cannot be null.");
        if (!reader.Read())
        {
            throw new JsonException("Double-spend-proof notification parameters must contain exactly two items.");
        }

        var proof = reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.StartObject => ((JsonConverter<ElectrumXDoubleSpendProof>)options.GetConverter(
                typeof(ElectrumXDoubleSpendProof))).Read(
                ref reader,
                typeof(ElectrumXDoubleSpendProof),
                options),
            _ => throw new JsonException("The double-spend proof must be an object or null.")
        };

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Double-spend-proof notification parameters must contain exactly two items.");
        }

        return new ElectrumXDoubleSpendProofNotification(transactionHash, proof);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXDoubleSpendProofNotification value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        if (value.DoubleSpendProof is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            var proofConverter = (JsonConverter<ElectrumXDoubleSpendProof>)options.GetConverter(typeof(ElectrumXDoubleSpendProof));
            proofConverter.Write(writer, value.DoubleSpendProof, options);
        }

        writer.WriteEndArray();
    }
}
