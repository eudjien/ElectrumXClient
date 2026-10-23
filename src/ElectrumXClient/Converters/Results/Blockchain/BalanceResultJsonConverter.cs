using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class BalanceResultJsonConverter : JsonConverter<ElectrumXBalance>
{
    public override bool HandleNull => true;

    public override ElectrumXBalance Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        long? confirmed = null;
        long? unconfirmed = null;
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a property name.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("Expected a property value.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.BalanceResult.Confirmed:
                    {
                        if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt64(out long balance))
                        {
                            throw new JsonException("The confirmed balance must be an integer.");
                        }

                        confirmed = balance;
                        break;
                    }
                case ElectrumXJsonPropertyNames.BalanceResult.Unconfirmed:
                    {
                        if (reader.TokenType != JsonTokenType.Number || !reader.TryGetInt64(out long balance))
                        {
                            throw new JsonException("The unconfirmed balance must be an integer.");
                        }

                        unconfirmed = balance;
                        break;
                    }
                default:
                    {
                        if (reader.TokenType is JsonTokenType.StartArray or JsonTokenType.StartObject)
                        {
                            reader.Skip();
                        }
                        break;
                    }
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject ||
            confirmed is not {} confirmedValue ||
            unconfirmed is not {} unconfirmedValue)
        {
            throw new JsonException("The balance response is incomplete.");
        }

        return new ElectrumXBalance(confirmedValue, unconfirmedValue);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXBalance value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.BalanceResult.Confirmed, value.Confirmed);
        writer.WriteNumber(ElectrumXJsonPropertyNames.BalanceResult.Unconfirmed, value.Unconfirmed);
        writer.WriteEndObject();
    }
}
