using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.ProTx;

public sealed class ProTxDiffResultJsonConverter : JsonConverter<ElectrumXProTxDiff>
{
    public override bool HandleNull => true;

    public override ElectrumXProTxDiff Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected a JSON object.");
        }

        string? baseBlockHash = null;
        string? blockHash = null;
        string? coinbaseTransactionMerkleTree = null;
        string? coinbaseTransaction = null;
        IReadOnlyList<string>? deletedMasterNodes = null;
        IReadOnlyList<ElectrumXProTxMasterNode>? masterNodes = null;
        string? masterNodeListMerkleRoot = null;
        var nodeConverter = (JsonConverter<ElectrumXProTxMasterNode>)options.GetConverter(typeof(ElectrumXProTxMasterNode));

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected a ProTx diff property.");
            }

            string? propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("The ProTx diff property value is missing.");
            }

            switch (propertyName)
            {
                case ElectrumXJsonPropertyNames.ProTxDiffResult.BaseBlockHash:
                    baseBlockHash = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxDiffResult.BlockHash:
                    blockHash = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxDiffResult.CoinbaseTransactionMerkleTree:
                    coinbaseTransactionMerkleTree = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxDiffResult.CoinbaseTransaction:
                    coinbaseTransaction = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                case ElectrumXJsonPropertyNames.ProTxDiffResult.DeletedMasterNodes:
                    {
                        if (reader.TokenType == JsonTokenType.Null)
                        {
                            deletedMasterNodes = null;
                        }
                        else
                        {
                            if (reader.TokenType != JsonTokenType.StartArray)
                            {
                                throw new JsonException($"Property '{propertyName}' must be an array or null.");
                            }

                            var deleted = new List<string>();
                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                if (reader.TokenType != JsonTokenType.String)
                                {
                                    throw new JsonException($"Items in '{propertyName}' must be strings.");
                                }

                                deleted.Add(reader.GetString()
                                            ?? throw new JsonException($"Items in '{propertyName}' must be strings."));
                            }

                            if (reader.TokenType != JsonTokenType.EndArray)
                            {
                                throw new JsonException($"The array property '{propertyName}' is incomplete.");
                            }

                            deletedMasterNodes = deleted;
                        }
                        break;
                    }
                case ElectrumXJsonPropertyNames.ProTxDiffResult.MasterNodes:
                    {
                        if (reader.TokenType == JsonTokenType.Null)
                        {
                            masterNodes = null;
                        }
                        else
                        {
                            if (reader.TokenType != JsonTokenType.StartArray)
                            {
                                throw new JsonException($"Property '{propertyName}' must be an array or null.");
                            }

                            var nodes = new List<ElectrumXProTxMasterNode>();
                            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                            {
                                nodes.Add(nodeConverter.Read(ref reader, typeof(ElectrumXProTxMasterNode), options)
                                          ?? throw new JsonException("A ProTx masternode entry cannot be null."));
                            }

                            if (reader.TokenType != JsonTokenType.EndArray)
                            {
                                throw new JsonException($"The array property '{propertyName}' is incomplete.");
                            }

                            masterNodes = nodes;
                        }
                        break;
                    }
                case ElectrumXJsonPropertyNames.ProTxDiffResult.MasterNodeListMerkleRoot:
                    masterNodeListMerkleRoot = reader.TokenType switch
                    {
                        JsonTokenType.Null => null,
                        JsonTokenType.String => reader.GetString()
                                                ?? throw new JsonException($"Property '{propertyName}' must be a string or null."),
                        _ => throw new JsonException($"Property '{propertyName}' must be a string or null.")
                    };
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("The ProTx diff object is incomplete.");
        }

        return new ElectrumXProTxDiff(
            baseBlockHash, blockHash, coinbaseTransactionMerkleTree, coinbaseTransaction, deletedMasterNodes,
            masterNodes, masterNodeListMerkleRoot);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXProTxDiff? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxDiffResult.BaseBlockHash, value.BaseBlockHash);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxDiffResult.BlockHash, value.BlockHash);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxDiffResult.CoinbaseTransactionMerkleTree, value.CoinbaseTransactionMerkleTree);
        writer.WriteString(ElectrumXJsonPropertyNames.ProTxDiffResult.CoinbaseTransaction, value.CoinbaseTransaction);
        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxDiffResult.DeletedMasterNodes);
        if (value.DeletedMasterNodes is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();
            foreach (string deletedNode in value.DeletedMasterNodes)
            {
                writer.WriteStringValue(deletedNode);
            }

            writer.WriteEndArray();
        }

        writer.WritePropertyName(ElectrumXJsonPropertyNames.ProTxDiffResult.MasterNodes);
        if (value.MasterNodes is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStartArray();
            var nodeConverter = (JsonConverter<ElectrumXProTxMasterNode>)options.GetConverter(typeof(ElectrumXProTxMasterNode));
            foreach (var node in value.MasterNodes)
            {
                nodeConverter.Write(writer, node, options);
            }

            writer.WriteEndArray();
        }

        writer.WriteString(ElectrumXJsonPropertyNames.ProTxDiffResult.MasterNodeListMerkleRoot, value.MasterNodeListMerkleRoot);
        writer.WriteEndObject();
    }
}
