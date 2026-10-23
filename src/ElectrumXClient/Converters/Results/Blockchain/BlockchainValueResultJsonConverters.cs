using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class FirstUseResultJsonConverter : JsonConverter<ElectrumXFirstUse>
{
    public override ElectrumXFirstUse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The first-use result must be an object.");
        }

        string? blockHash = null;
        long? height = null;
        string? transactionHash = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
            {
                throw new JsonException("The first-use result is incomplete.");
            }

            switch (reader.GetString())
            {
                case ElectrumXJsonPropertyNames.FirstUseResult.BlockHash:
                    blockHash = ReadString(ref reader, "The first-use block hash must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.FirstUseResult.Height:
                    height = ReadInt64(ref reader, "The first-use block height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.FirstUseResult.TransactionHash:
                    transactionHash = ReadString(ref reader, "The first-use transaction hash must be a string.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The first-use result is incomplete.");
        }

        return new ElectrumXFirstUse(
            blockHash ?? throw new JsonException("The first-use block hash is missing."),
            height ?? throw new JsonException("The first-use block height is missing."),
            transactionHash ?? throw new JsonException("The first-use transaction hash is missing."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXFirstUse value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.FirstUseResult.BlockHash, value.BlockHash);
        writer.WriteNumber(ElectrumXJsonPropertyNames.FirstUseResult.Height, value.Height);
        writer.WriteString(ElectrumXJsonPropertyNames.FirstUseResult.TransactionHash, value.TransactionHash);
        writer.WriteEndObject();
    }

    internal static string ReadString(ref Utf8JsonReader reader, string message)
    {
        return reader.TokenType == JsonTokenType.String
            ? reader.GetString() ?? throw new JsonException(message)
            : throw new JsonException(message);
    }

    internal static long ReadInt64(ref Utf8JsonReader reader, string message)
    {
        return reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long value)
            ? value
            : throw new JsonException(message);
    }
}

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
            if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
            {
                throw new JsonException("The UTXO info result is incomplete.");
            }

            switch (reader.GetString())
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

public sealed class ConfirmedTransactionBlockHashResultJsonConverter : JsonConverter<ElectrumXConfirmedTransactionBlockHash>
{
    public override ElectrumXConfirmedTransactionBlockHash Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The confirmed transaction block result must be an object.");
        }

        string? blockHash = null;
        long? blockHeight = null;
        string? blockHeader = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
            {
                throw new JsonException("The confirmed transaction block result is incomplete.");
            }

            switch (reader.GetString())
            {
                case ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHash:
                    blockHash = FirstUseResultJsonConverter.ReadString(ref reader, "The block hash must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHeight:
                    blockHeight = FirstUseResultJsonConverter.ReadInt64(ref reader, "The block height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHeader:
                    blockHeader = reader.TokenType == JsonTokenType.Null
                        ? null
                        : FirstUseResultJsonConverter.ReadString(ref reader, "The block header must be a string.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The confirmed transaction block result is incomplete.");
        }

        return new ElectrumXConfirmedTransactionBlockHash(
            blockHash ?? throw new JsonException("The confirmed transaction block hash is missing."),
            blockHeight ?? throw new JsonException("The confirmed transaction block height is missing."),
            blockHeader);
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXConfirmedTransactionBlockHash value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHash, value.BlockHash);
        writer.WriteNumber(ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHeight, value.BlockHeight);
        if (value.BlockHeader is not null)
        {
            writer.WriteString(ElectrumXJsonPropertyNames.ConfirmedTransactionBlockHashResult.BlockHeader, value.BlockHeader);
        }

        writer.WriteEndObject();
    }
}

public sealed class HeaderInfoResultJsonConverter : JsonConverter<ElectrumXHeaderInfo>
{
    public override ElectrumXHeaderInfo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("The header info result must be an object.");
        }

        long? height = null;
        string? hex = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
            {
                throw new JsonException("The header info result is incomplete.");
            }

            switch (reader.GetString())
            {
                case ElectrumXJsonPropertyNames.HeaderInfoResult.Height:
                    height = FirstUseResultJsonConverter.ReadInt64(ref reader, "The header height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.HeaderInfoResult.Hex:
                    hex = FirstUseResultJsonConverter.ReadString(ref reader, "The header hex must be a string.");
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The header info result is incomplete.");
        }

        return new ElectrumXHeaderInfo(
            height ?? throw new JsonException("The header height is missing."),
            hex ?? throw new JsonException("The header hex is missing."));
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXHeaderInfo value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.HeaderInfoResult.Height, value.Height);
        writer.WriteString(ElectrumXJsonPropertyNames.HeaderInfoResult.Hex, value.Hex);
        writer.WriteEndObject();
    }
}
