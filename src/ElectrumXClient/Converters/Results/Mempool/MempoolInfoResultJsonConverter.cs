using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Mempool;

public sealed class MempoolInfoResultJsonConverter : JsonConverter<ElectrumXMempoolInfo>
{
    public override ElectrumXMempoolInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The mempool info result must be an object.");
        }

        decimal? mempoolMinimumFee = null;
        decimal? minimumRelayFee = null;
        decimal? incrementalRelayFee = null;
        int? unbroadcastCount = null;
        bool? fullReplaceByFee = null;
        var additionalProperties = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);
        var jsonValueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("The mempool info result is incomplete.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The mempool info result is incomplete.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.MempoolInfoResult.MempoolMinimumFee:
                    mempoolMinimumFee = ReadDecimal(ref reader);
                    break;
                case ElectrumXJsonPropertyNames.MempoolInfoResult.MinimumRelayFee:
                    minimumRelayFee = ReadDecimal(ref reader);
                    break;
                case ElectrumXJsonPropertyNames.MempoolInfoResult.IncrementalRelayFee:
                    incrementalRelayFee = ReadDecimal(ref reader);
                    break;
                case ElectrumXJsonPropertyNames.MempoolInfoResult.UnbroadcastCount:
                    unbroadcastCount = reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int count)
                        ? count
                        : throw new JsonException("The mempool unbroadcast count must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.MempoolInfoResult.FullReplaceByFee:
                    fullReplaceByFee = reader.TokenType switch
                    {
                        JsonTokenType.True => true,
                        JsonTokenType.False => false,
                        _ => throw new JsonException("The full-RBF value must be a boolean.")
                    };
                    break;
                default:
                    string name = propertyName
                                  ?? throw new JsonException("A mempool info property name cannot be null.");
                    additionalProperties.Add(
                        name,
                        jsonValueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options)
                        ?? throw new JsonException($"Mempool info property '{name}' cannot be null."));
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The mempool info result is incomplete.");
        }

        return new ElectrumXMempoolInfo(
            mempoolMinimumFee,
            minimumRelayFee,
            incrementalRelayFee,
            unbroadcastCount,
            fullReplaceByFee,
            additionalProperties);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXMempoolInfo value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.MempoolMinimumFee is {} mempoolMinimumFee)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.MempoolInfoResult.MempoolMinimumFee, mempoolMinimumFee);
        }

        if (value.MinimumRelayFee is {} minimumRelayFee)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.MempoolInfoResult.MinimumRelayFee, minimumRelayFee);
        }

        if (value.IncrementalRelayFee is {} incrementalRelayFee)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.MempoolInfoResult.IncrementalRelayFee, incrementalRelayFee);
        }

        if (value.UnbroadcastCount is {} unbroadcastCount)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.MempoolInfoResult.UnbroadcastCount, unbroadcastCount);
        }

        if (value.FullReplaceByFee is {} fullReplaceByFee)
        {
            writer.WriteBoolean(ElectrumXJsonPropertyNames.MempoolInfoResult.FullReplaceByFee, fullReplaceByFee);
        }

        var jsonValueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
        foreach ((string name, var propertyValue) in value.AdditionalProperties)
        {
            writer.WritePropertyName(name);
            jsonValueConverter.Write(writer, propertyValue, options);
        }

        writer.WriteEndObject();
    }

    private static decimal ReadDecimal(ref Utf8JsonReader reader)
    {
        return reader.TokenType == JsonTokenType.Number && reader.TryGetDecimal(out decimal value)
            ? value
            : throw new JsonException("A mempool fee rate must be a number.");
    }
}
