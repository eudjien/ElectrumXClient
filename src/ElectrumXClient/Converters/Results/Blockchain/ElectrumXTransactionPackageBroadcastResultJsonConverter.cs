using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class ElectrumXTransactionPackageBroadcastResultJsonConverter
    : JsonConverter<ElectrumXTransactionPackageBroadcast>
{
    public override bool HandleNull => true;

    public override ElectrumXTransactionPackageBroadcast Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The package broadcast result must be an object.");
        }

        bool success = false;
        bool hasSuccess = false;
        List<ElectrumXTransactionPackageError>? errors = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a package broadcast result property name.");
            }

            string propertyName = reader.GetString()
                                  ?? throw new JsonException("A package broadcast property name cannot be null.");
            if (!reader.Read())
            {
                throw new JsonException("A package broadcast result property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.Success:
                    if (reader.TokenType is not (JsonTokenType.True or JsonTokenType.False))
                    {
                        throw new JsonException("Package broadcast success must be a boolean.");
                    }

                    success = reader.GetBoolean();
                    hasSuccess = true;
                    break;
                case ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.Errors:
                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        errors = null;
                        break;
                    }

                    if (reader.TokenType != JsonTokenType.StartArray)
                    {
                        throw new JsonException("Package broadcast errors must be an array or null.");
                    }

                    errors = [];
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (reader.TokenType != JsonTokenType.StartObject)
                        {
                            throw new JsonException("A package broadcast error must be an object.");
                        }

                        string? transactionId = null;
                        string? error = null;
                        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                        {
                            if (reader.TokenType != JsonTokenType.PropertyName)
                            {
                                throw new JsonException("Expected a package broadcast error property name.");
                            }

                            string errorPropertyName = reader.GetString()
                                                       ?? throw new JsonException("A package broadcast error property name cannot be null.");
                            if (!reader.Read())
                            {
                                throw new JsonException("A package broadcast error property value is missing.");
                            }

                            switch (errorPropertyName)
                            {
                                case ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.TransactionId:
                                    transactionId = reader.TokenType == JsonTokenType.String
                                        ? reader.GetString()
                                        : throw new JsonException(
                                            $"Package broadcast error property '{errorPropertyName}' must be a string.");
                                    break;
                                case ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.Error:
                                    error = reader.TokenType == JsonTokenType.String
                                        ? reader.GetString()
                                        : throw new JsonException(
                                            $"Package broadcast error property '{errorPropertyName}' must be a string.");
                                    break;
                                default:
                                    reader.Skip();
                                    break;
                            }
                        }

                        if (reader.TokenType != JsonTokenType.EndObject)
                        {
                            throw new JsonException("A package broadcast error object is incomplete.");
                        }

                        errors.Add(new ElectrumXTransactionPackageError(transactionId, error));
                    }

                    if (reader.TokenType != JsonTokenType.EndArray)
                    {
                        throw new JsonException("The package broadcast errors array is incomplete.");
                    }
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The package broadcast result object is incomplete.");
        }

        if (!hasSuccess)
        {
            throw new JsonException(
                $"Required property '{ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.Success}' is missing.");
        }

        return new ElectrumXTransactionPackageBroadcast(success, errors);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXTransactionPackageBroadcast? value,
        JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteBoolean(ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.Success, value.Success);

        if (value.Errors is not null)
        {
            writer.WritePropertyName(ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.Errors);
            writer.WriteStartArray();
            foreach (var error in value.Errors)
            {
                writer.WriteStartObject();
                if (error.TransactionId is not null)
                {
                    writer.WriteString(ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.TransactionId, error.TransactionId);
                }

                if (error.Error is not null)
                {
                    writer.WriteString(ElectrumXJsonPropertyNames.TransactionPackageBroadcastResult.Error, error.Error);
                }

                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}
