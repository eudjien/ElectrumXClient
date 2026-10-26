using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Globalization;
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
                        : reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long parsedHeight)
                            ? parsedHeight
                            : throw new JsonException("The confirmed height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.UtxoInfoResult.ScriptHash:
                    scriptHash = reader.TokenType == JsonTokenType.String
                        ? reader.GetString() ?? throw new JsonException("The UTXO script hash must be a string.")
                        : throw new JsonException("The UTXO script hash must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.UtxoInfoResult.Value:
                    value = reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long parsedValue)
                        ? parsedValue
                        : throw new JsonException("The UTXO value must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.UtxoInfoResult.TokenData:
                    if (reader.TokenType == JsonTokenType.Null)
                    {
                        tokenData = null;
                        break;
                    }

                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException("Token data must be an object or null.");
                    }

                    long? tokenAmount = null;
                    string? category = null;
                    ElectrumXTokenNft? nft = null;
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                    {
                        if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
                        {
                            throw new JsonException("Token data is incomplete.");
                        }

                        switch (reader.GetString())
                        {
                            case ElectrumXJsonPropertyNames.TokenDataResult.Amount:
                                if (reader.TokenType != JsonTokenType.String ||
                                    !long.TryParse(
                                        reader.GetString(),
                                        NumberStyles.None,
                                        CultureInfo.InvariantCulture,
                                        out long parsedTokenAmount))
                                {
                                    throw new JsonException("The token amount must be a non-negative integer string within the Int64 range.");
                                }

                                tokenAmount = parsedTokenAmount;
                                break;
                            case ElectrumXJsonPropertyNames.TokenDataResult.Category:
                                category = reader.TokenType == JsonTokenType.String
                                    ? reader.GetString()
                                    : throw new JsonException("The token category must be a string.");
                                break;
                            case ElectrumXJsonPropertyNames.TokenDataResult.Nft:
                                if (reader.TokenType == JsonTokenType.Null)
                                {
                                    nft = null;
                                    break;
                                }

                                if (reader.TokenType != JsonTokenType.StartObject)
                                {
                                    throw new JsonException("NFT token data must be an object or null.");
                                }

                                ElectrumXTokenNftCapability? capability = null;
                                string? commitment = null;
                                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                                {
                                    if (reader.TokenType != JsonTokenType.PropertyName || !reader.Read())
                                    {
                                        throw new JsonException("NFT token data is incomplete.");
                                    }

                                    switch (reader.GetString())
                                    {
                                        case ElectrumXJsonPropertyNames.TokenDataResult.Capability:
                                            capability = reader.TokenType == JsonTokenType.String
                                                ? reader.GetString() switch
                                                {
                                                    ElectrumXJsonPropertyNames.TokenDataResult.CapabilityNone =>
                                                        ElectrumXTokenNftCapability.None,
                                                    ElectrumXJsonPropertyNames.TokenDataResult.CapabilityMutable =>
                                                        ElectrumXTokenNftCapability.Mutable,
                                                    ElectrumXJsonPropertyNames.TokenDataResult.CapabilityMinting =>
                                                        ElectrumXTokenNftCapability.Minting,
                                                    _ => throw new JsonException("The NFT capability is not recognized.")
                                                }
                                                : throw new JsonException("The NFT capability must be a string.");
                                            break;
                                        case ElectrumXJsonPropertyNames.TokenDataResult.Commitment:
                                            commitment = reader.TokenType == JsonTokenType.String
                                                ? reader.GetString()
                                                  ?? throw new JsonException("The NFT commitment must be a string.")
                                                : throw new JsonException("The NFT commitment must be a string.");
                                            break;
                                        default:
                                            reader.Skip();
                                            break;
                                    }
                                }

                                if (reader.TokenType != JsonTokenType.EndObject)
                                {
                                    throw new JsonException("NFT token data is incomplete.");
                                }

                                nft = new ElectrumXTokenNft(
                                    capability ?? throw new JsonException("The NFT capability is missing."),
                                    commitment ?? throw new JsonException("The NFT commitment is missing."));
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }

                    if (reader.TokenType != JsonTokenType.EndObject)
                    {
                        throw new JsonException("Token data is incomplete.");
                    }

                    tokenData = new ElectrumXTokenData(
                        tokenAmount ?? throw new JsonException("The token amount is missing."),
                        category ?? throw new JsonException("The token category is missing."),
                        nft);
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
            writer.WritePropertyName(ElectrumXJsonPropertyNames.UtxoInfoResult.TokenData);
            writer.WriteStartObject();
            writer.WriteString(
                ElectrumXJsonPropertyNames.TokenDataResult.Amount,
                value.TokenData.Amount.ToString(CultureInfo.InvariantCulture));
            writer.WriteString(ElectrumXJsonPropertyNames.TokenDataResult.Category, value.TokenData.Category);
            if (value.TokenData.Nft is not null)
            {
                writer.WritePropertyName(ElectrumXJsonPropertyNames.TokenDataResult.Nft);
                writer.WriteStartObject();
                writer.WriteString(
                    ElectrumXJsonPropertyNames.TokenDataResult.Capability,
                    value.TokenData.Nft.Capability switch
                    {
                        ElectrumXTokenNftCapability.None => ElectrumXJsonPropertyNames.TokenDataResult.CapabilityNone,
                        ElectrumXTokenNftCapability.Mutable => ElectrumXJsonPropertyNames.TokenDataResult.CapabilityMutable,
                        ElectrumXTokenNftCapability.Minting => ElectrumXJsonPropertyNames.TokenDataResult.CapabilityMinting,
                        _ => throw new JsonException("The NFT capability is not recognized.")
                    });
                writer.WriteString(ElectrumXJsonPropertyNames.TokenDataResult.Commitment, value.TokenData.Nft.Commitment);
                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }

        writer.WriteEndObject();
    }
}
