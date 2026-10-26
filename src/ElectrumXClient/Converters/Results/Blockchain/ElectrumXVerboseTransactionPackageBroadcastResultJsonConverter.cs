using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class ElectrumXVerboseTransactionPackageBroadcastResultJsonConverter
    : JsonConverter<ElectrumXVerboseTransactionPackageBroadcast>
{
    public override bool HandleNull => true;

    public override ElectrumXVerboseTransactionPackageBroadcast Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The verbose package broadcast result must be an object.");
        }

        string? packageMessage = null;
        Dictionary<string, ElectrumXVerboseTransactionPackageTransactionEntry> transactionResults =
            new(StringComparer.Ordinal);
        List<string>? replacedTransactions = null;
        Dictionary<string, ElectrumXJsonValue> additionalProperties = new(StringComparer.Ordinal);
        var jsonValueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a verbose package broadcast result property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("A verbose package broadcast property name cannot be null.");
            if (!reader.Read())
            {
                throw new JsonException("A verbose package broadcast result property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.PackageMessage:
                    packageMessage = reader.TokenType switch
                    {
                        JsonTokenType.String => reader.GetString(),
                        JsonTokenType.Null => null,
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.TransactionResults:
                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException("The verbose transaction results must be an object.");
                    }

                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType != JsonTokenType.PropertyName)
                        {
                            throw new JsonException("Expected a verbose transaction result property name.");
                        }

                        string witnessTransactionId = reader.GetString()
                                                      ?? throw new JsonException("A witness transaction ID cannot be null.");
                        if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                        {
                            throw new JsonException("A verbose transaction result must be an object.");
                        }

                        string? transactionId = null;
                        string? otherWitnessTransactionId = null;
                        long? virtualSize = null;
                        ElectrumXVerboseTransactionPackageFees? fees = null;
                        string? error = null;
                        Dictionary<string, ElectrumXJsonValue> transactionAdditionalProperties = new(StringComparer.Ordinal);

                        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                        {
                            if (reader.TokenType != JsonTokenType.PropertyName)
                            {
                                throw new JsonException("Expected a verbose transaction property name.");
                            }

                            string transactionPropertyName = reader.GetString()
                                                             ?? throw new JsonException("A verbose transaction property name cannot be null.");
                            if (!reader.Read())
                            {
                                throw new JsonException("A verbose transaction property value is missing.");
                            }

                            switch (transactionPropertyName)
                            {
                                case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.TransactionId:
                                    transactionId = reader.TokenType switch
                                    {
                                        JsonTokenType.String => reader.GetString(),
                                        JsonTokenType.Null => null,
                                        _ => throw new JsonException(
                                            $"Property '{transactionPropertyName}' must be a string or null.")
                                    };
                                    break;
                                case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.OtherWitnessTransactionId:
                                    otherWitnessTransactionId = reader.TokenType switch
                                    {
                                        JsonTokenType.String => reader.GetString(),
                                        JsonTokenType.Null => null,
                                        _ => throw new JsonException(
                                            $"Property '{transactionPropertyName}' must be a string or null.")
                                    };
                                    break;
                                case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.VirtualSize:
                                    virtualSize = reader.TokenType switch
                                    {
                                        JsonTokenType.Number when reader.TryGetInt64(out long size) => size,
                                        JsonTokenType.Null => null,
                                        _ => throw new JsonException(
                                            $"Property '{transactionPropertyName}' must be a 64-bit integer or null.")
                                    };
                                    break;
                                case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.Fees:
                                    if (reader.TokenType != JsonTokenType.StartObject)
                                    {
                                        throw new JsonException("Verbose transaction fees must be an object.");
                                    }

                                    decimal? baseFee = null;
                                    decimal? effectiveFeeRate = null;
                                    List<string>? effectiveIncludes = null;
                                    Dictionary<string, ElectrumXJsonValue> feeAdditionalProperties = new(StringComparer.Ordinal);

                                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                                    {
                                        if (reader.TokenType != JsonTokenType.PropertyName)
                                        {
                                            throw new JsonException("Expected a verbose transaction fees property name.");
                                        }

                                        string feePropertyName = reader.GetString()
                                                                 ?? throw new JsonException("A verbose transaction fees property name cannot be null.");
                                        if (!reader.Read())
                                        {
                                            throw new JsonException("A verbose transaction fees property value is missing.");
                                        }

                                        switch (feePropertyName)
                                        {
                                            case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.BaseFee:
                                                baseFee = reader.TokenType switch
                                                {
                                                    JsonTokenType.Number when reader.TryGetDecimal(out decimal parsedBaseFee) => parsedBaseFee,
                                                    JsonTokenType.Null => null,
                                                    _ => throw new JsonException(
                                                        $"Property '{feePropertyName}' must be a decimal number or null.")
                                                };
                                                break;
                                            case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.EffectiveFeeRate:
                                                effectiveFeeRate = reader.TokenType switch
                                                {
                                                    JsonTokenType.Number when reader.TryGetDecimal(out decimal parsedFeeRate) => parsedFeeRate,
                                                    JsonTokenType.Null => null,
                                                    _ => throw new JsonException(
                                                        $"Property '{feePropertyName}' must be a decimal number or null.")
                                                };
                                                break;
                                            case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.EffectiveIncludes:
                                                if (reader.TokenType == JsonTokenType.Null)
                                                {
                                                    effectiveIncludes = null;
                                                    break;
                                                }

                                                if (reader.TokenType != JsonTokenType.StartArray)
                                                {
                                                    throw new JsonException($"Property '{feePropertyName}' must be an array or null.");
                                                }

                                                effectiveIncludes = [];
                                                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                                                {
                                                    if (reader.TokenType != JsonTokenType.String)
                                                    {
                                                        throw new JsonException(
                                                            $"Property '{feePropertyName}' must contain only strings.");
                                                    }

                                                    effectiveIncludes.Add(reader.GetString()
                                                                          ?? throw new JsonException(
                                                                              $"Property '{feePropertyName}' cannot contain null values."));
                                                }

                                                if (reader.TokenType != JsonTokenType.EndArray)
                                                {
                                                    throw new JsonException($"Property '{feePropertyName}' array is incomplete.");
                                                }

                                                break;
                                            default:
                                                feeAdditionalProperties.Add(
                                                    feePropertyName,
                                                    jsonValueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options)
                                                    ?? throw new JsonException(
                                                        $"Additional property '{feePropertyName}' cannot be null."));
                                                break;
                                        }
                                    }

                                    if (reader.TokenType != JsonTokenType.EndObject)
                                    {
                                        throw new JsonException("The verbose transaction fees object is incomplete.");
                                    }

                                    fees = new ElectrumXVerboseTransactionPackageFees(
                                        baseFee,
                                        effectiveFeeRate,
                                        effectiveIncludes,
                                        feeAdditionalProperties);
                                    break;
                                case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.Error:
                                    error = reader.TokenType switch
                                    {
                                        JsonTokenType.String => reader.GetString(),
                                        JsonTokenType.Null => null,
                                        _ => throw new JsonException(
                                            $"Property '{transactionPropertyName}' must be a string or null.")
                                    };
                                    break;
                                default:
                                    transactionAdditionalProperties.Add(
                                        transactionPropertyName,
                                        jsonValueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options)
                                        ?? throw new JsonException(
                                            $"Additional property '{transactionPropertyName}' cannot be null."));
                                    break;
                            }
                        }

                        if (reader.TokenType != JsonTokenType.EndObject)
                        {
                            throw new JsonException("A verbose transaction result object is incomplete.");
                        }

                        transactionResults.Add(
                            witnessTransactionId,
                            new ElectrumXVerboseTransactionPackageTransactionEntry(
                                transactionId,
                                otherWitnessTransactionId,
                                virtualSize,
                                fees,
                                error,
                                transactionAdditionalProperties));
                    }

                    if (reader.TokenType != JsonTokenType.EndObject)
                    {
                        throw new JsonException("The verbose transaction results object is incomplete.");
                    }
                    break;
                case ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.ReplacedTransactions:
                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        replacedTransactions = null;
                        break;
                    }

                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        throw new JsonException($"Property '{propertyName}' must be an array or null.");
                    }

                    replacedTransactions = [];
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException($"Property '{propertyName}' must contain only strings.");
                        }

                        replacedTransactions.Add(reader.GetString()
                                                 ?? throw new JsonException($"Property '{propertyName}' cannot contain null values."));
                    }

                    if (reader.TokenType != JsonTokenType.EndArray)
                    {
                        throw new JsonException($"Property '{propertyName}' array is incomplete.");
                    }
                    break;
                default:
                    additionalProperties.Add(
                        propertyName,
                        jsonValueConverter.Read(ref reader, typeof(ElectrumXJsonValue), options)
                        ?? throw new JsonException($"Additional property '{propertyName}' cannot be null."));
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The verbose package broadcast result object is incomplete.");
        }

        return new ElectrumXVerboseTransactionPackageBroadcast(
            packageMessage,
            transactionResults,
            replacedTransactions,
            additionalProperties);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXVerboseTransactionPackageBroadcast? value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        if (value.PackageMessage is not null)
        {
            writer.WriteString(
                ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.PackageMessage,
                value.PackageMessage);
        }

        writer.WritePropertyName(
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.TransactionResults);
        writer.WriteStartObject();
        var jsonValueConverter = (JsonConverter<ElectrumXJsonValue>)options.GetConverter(typeof(ElectrumXJsonValue));
        foreach ((string witnessTransactionId, var result) in value.TransactionResults)
        {
            writer.WritePropertyName(witnessTransactionId);
            writer.WriteStartObject();
            if (result.TransactionId is not null)
            {
                writer.WriteString(
                    ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.TransactionId,
                    result.TransactionId);
            }

            if (result.OtherWitnessTransactionId is not null)
            {
                writer.WriteString(
                    ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.OtherWitnessTransactionId,
                    result.OtherWitnessTransactionId);
            }

            if (result.VirtualSize is {} virtualSize)
            {
                writer.WriteNumber(
                    ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.VirtualSize,
                    virtualSize);
            }

            if (result.Fees is not null)
            {
                writer.WritePropertyName(ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.Fees);
                writer.WriteStartObject();
                if (result.Fees.BaseFee is {} baseFee)
                {
                    writer.WriteNumber(ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.BaseFee, baseFee);
                }

                if (result.Fees.EffectiveFeeRate is {} effectiveFeeRate)
                {
                    writer.WriteNumber(
                        ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.EffectiveFeeRate,
                        effectiveFeeRate);
                }

                if (result.Fees.EffectiveIncludes is not null)
                {
                    writer.WritePropertyName(
                        ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.EffectiveIncludes);
                    writer.WriteStartArray();
                    foreach (string transaction in result.Fees.EffectiveIncludes)
                    {
                        writer.WriteStringValue(transaction);
                    }

                    writer.WriteEndArray();
                }

                foreach ((string name, var propertyValue) in result.Fees.AdditionalProperties)
                {
                    if (name is ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.BaseFee or
                        ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.EffectiveFeeRate or
                        ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.EffectiveIncludes)
                    {
                        continue;
                    }

                    writer.WritePropertyName(name);
                    jsonValueConverter.Write(writer, propertyValue, options);
                }

                writer.WriteEndObject();
            }

            if (result.Error is not null)
            {
                writer.WriteString(
                    ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.Error,
                    result.Error);
            }

            foreach ((string name, var propertyValue) in result.AdditionalProperties)
            {
                if (name is ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.TransactionId or
                    ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.OtherWitnessTransactionId or
                    ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.VirtualSize or
                    ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.Fees or
                    ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.Error)
                {
                    continue;
                }

                writer.WritePropertyName(name);
                jsonValueConverter.Write(writer, propertyValue, options);
            }

            writer.WriteEndObject();
        }

        writer.WriteEndObject();

        if (value.ReplacedTransactions is not null)
        {
            writer.WritePropertyName(
                ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.ReplacedTransactions);
            writer.WriteStartArray();
            foreach (string transaction in value.ReplacedTransactions)
            {
                writer.WriteStringValue(transaction);
            }

            writer.WriteEndArray();
        }

        foreach ((string name, var propertyValue) in value.AdditionalProperties)
        {
            if (name is ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.PackageMessage or
                ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.TransactionResults or
                ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.ReplacedTransactions)
            {
                continue;
            }

            writer.WritePropertyName(name);
            jsonValueConverter.Write(writer, propertyValue, options);
        }

        writer.WriteEndObject();
    }
}
