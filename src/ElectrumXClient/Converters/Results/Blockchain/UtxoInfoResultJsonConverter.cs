using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class UtxoInfoResultJsonConverter : JsonConverter<ElectrumXUtxoInfo>
{
    public override ElectrumXUtxoInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The UTXO info result must be an object.");
        }

        long? confirmedHeight = null;
        string? scriptHash = null;
        long? value = null;
        ElectrumXTokenData? tokenData = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("The UTXO info result is incomplete.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The UTXO info result is incomplete.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.UtxoInfoResult.ConfirmedHeight:
                    confirmedHeight = reader.TokenType == JsonTokenType.Null
                        ? null
                        : FirstUseResultJsonConverter.ReadInt64(ref reader, "The confirmed height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.UtxoInfoResult.ScriptHash:
                    scriptHash = FirstUseResultJsonConverter.ReadString(ref reader, "The UTXO script hash must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.UtxoInfoResult.Value:
                    value = FirstUseResultJsonConverter.ReadInt64(ref reader, "The UTXO value must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.UtxoInfoResult.TokenData:
                    tokenData = reader.TokenType == JsonTokenType.Null
                        ? null
                        : UnspentOutputResultJsonConverter.ReadTokenData(ref reader);
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The UTXO info result is incomplete.");
        }

        return new ElectrumXUtxoInfo(
            confirmedHeight,
            scriptHash ?? throw new JsonException("The UTXO script hash is missing."),
            value ?? throw new JsonException("The UTXO value is missing."),
            tokenData);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXUtxoInfo value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.ConfirmedHeight is {} confirmedHeight)
        {
            writer.WriteNumber(ElectrumXJsonPropertyNames.UtxoInfoResult.ConfirmedHeight, confirmedHeight);
        }

        writer.WriteString(ElectrumXJsonPropertyNames.UtxoInfoResult.ScriptHash, value.ScriptHash);
        writer.WriteNumber(ElectrumXJsonPropertyNames.UtxoInfoResult.Value, value.Value);
        if (value.TokenData is not null)
        {
            UnspentOutputResultJsonConverter.WriteTokenData(writer, value.TokenData);
        }

        writer.WriteEndObject();
    }
}