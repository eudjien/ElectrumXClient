using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class UnspentOutputResultJsonConverter : JsonConverter<ElectrumXUnspentOutput>
{
    public override bool HandleNull => true;

    public override ElectrumXUnspentOutput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        long? height = null;
        int? transactionPosition = null;
        string? transactionHash = null;
        long? value = null;
        ElectrumXTokenData? tokenData = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected an unspent output property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The unspent output property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.UnspentOutputResult.Height:
                    height = reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long outputHeight)
                        ? outputHeight
                        : throw new JsonException("The unspent output height must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.UnspentOutputResult.TransactionPosition:
                    transactionPosition = reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int outputPosition)
                        ? outputPosition
                        : throw new JsonException("The transaction position must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.UnspentOutputResult.TransactionHash:
                    transactionHash = reader.TokenType == JsonTokenType.String
                        ? reader.GetString()
                        : throw new JsonException("The transaction hash must be a string.");
                    break;
                case ElectrumXJsonPropertyNames.UnspentOutputResult.Value:
                    value = reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long outputValue)
                        ? outputValue
                        : throw new JsonException("The unspent output value must be an integer.");
                    break;
                case ElectrumXJsonPropertyNames.UnspentOutputResult.TokenData:
                    if (reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException("Token data must be an object.");
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
            throw new JsonException("The unspent output object is incomplete.");
        }

        return new ElectrumXUnspentOutput(
            height ?? throw new JsonException("The unspent output height is missing."),
            transactionPosition ?? throw new JsonException("The unspent output position is missing."),
            transactionHash ?? throw new JsonException("The transaction hash is missing."),
            value ?? throw new JsonException("The unspent output value is missing."),
            tokenData);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXUnspentOutput value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteNumber(ElectrumXJsonPropertyNames.UnspentOutputResult.Height, value.Height);
        writer.WriteNumber(ElectrumXJsonPropertyNames.UnspentOutputResult.TransactionPosition, value.TransactionPosition);
        writer.WriteString(ElectrumXJsonPropertyNames.UnspentOutputResult.TransactionHash, value.TransactionHash);
        writer.WriteNumber(ElectrumXJsonPropertyNames.UnspentOutputResult.Value, value.Value);
        if (value.TokenData is not null)
        {
            writer.WritePropertyName(ElectrumXJsonPropertyNames.UnspentOutputResult.TokenData);
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
