using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.ProTx;

public sealed class ProTxInfoResultJsonConverter : JsonConverter<ElectrumXProTxInfo>
{
    public override bool HandleNull => true;

    public override ElectrumXProTxInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The ProTx info response must be an object.");
        }

        string? proTxHash = null;
        string? collateralHash = null;
        long? collateralIndex = null;
        decimal? operatorReward = null;
        ElectrumXProTxState? state = null;
        long? confirmations = null;
        var stateConverter = (JsonConverter<ElectrumXProTxState>)options.GetConverter(typeof(ElectrumXProTxState));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a ProTx info property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("Expected a ProTx info property name.");
            if (!reader.Read())
            {
                throw new JsonException("Expected a ProTx info property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.ProTxInfoResult.ProTxHash:
                    proTxHash = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxInfoResult.CollateralHash:
                    collateralHash = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxInfoResult.CollateralIndex:
                    collateralIndex = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxInfoResult.OperatorReward:
                    operatorReward = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetDecimal(out decimal value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be a number or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxInfoResult.State:
                    state = reader.TokenType == JsonTokenType.Null
                        ? null
                        : stateConverter.Read(ref reader, typeof(ElectrumXProTxState), options);
                    break;
                case ElectrumXJsonPropertyNames.ProTxInfoResult.Confirmations:
                    confirmations = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.Number when reader.TryGetInt64(out long value) => value,
                        _ => throw new JsonException($"Property '{propertyName}' must be an integer or null.")
                    };
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The ProTx info response is incomplete.");
        }

        return new ElectrumXProTxInfo(proTxHash, collateralHash, collateralIndex, operatorReward, state, confirmations);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXProTxInfo value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxInfoResult.ProTxHash, value.ProTxHash);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxInfoResult.CollateralHash, value.CollateralHash);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxInfoResult.CollateralIndex);
        if (value.CollateralIndex is {} collateralIndex)
        {
            writer.WriteNumberValue(collateralIndex);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxInfoResult.OperatorReward);
        if (value.OperatorReward is {} operatorReward)
        {
            writer.WriteNumberValue(operatorReward);
        }
        else
        {
            writer.WriteNullValue();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxInfoResult.State);
        if (value.State is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            var stateConverter = (JsonConverter<ElectrumXProTxState>)options.GetConverter(typeof(ElectrumXProTxState));
            stateConverter.Write(writer, value.State, options);
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxInfoResult.Confirmations);
        if (value.Confirmations is {} confirmations)
        {
            writer.WriteNumberValue(confirmations);
        }
        else
        {
            writer.WriteNullValue();
        }
        writer.WriteEndObject();
    }
}
