using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Coin-specific verbose transaction result returned when blockchain.transaction.get uses verbose=true.</summary>
public sealed class ElectrumXVerboseTransaction : ElectrumXResult
{
    /// <summary>Creates a coin-specific verbose transaction result.</summary>
    public ElectrumXVerboseTransaction(string? blockHash = null, long? blockTime = null, long? confirmations = null, string? hash = null,
        string? hex = null, long? lockTime = null, long? size = null, long? time = null, string? transactionId = null, long? version = null,
        IReadOnlyList<ElectrumXVerboseTransactionInput>? inputs = null, IReadOnlyList<ElectrumXVerboseTransactionOutput>? outputs = null,
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null)
    {
        BlockHash = blockHash;
        BlockTime = blockTime;
        Confirmations = confirmations;
        Hash = hash;
        Hex = hex;
        LockTime = lockTime;
        Size = size;
        Time = time;
        TransactionId = transactionId;
        Version = version;
        Inputs = inputs;
        Outputs = outputs;

        var additionalPropertiesCopy = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);
        if (additionalProperties is not null)
        {
            foreach ((string name, var value) in additionalProperties)
            {
                if (name is null)
                {
                    throw new ArgumentException("Additional verbose transaction property names cannot be null.", nameof(additionalProperties));
                }

                if (IsKnownProperty(name))
                {
                    throw new ArgumentException(
                        $"'{name}' is a standard verbose transaction property and cannot be an additional property.",
                        nameof(additionalProperties));
                }

                additionalPropertiesCopy.Add(
                    name,
                    value ?? throw new ArgumentException(
                        "Additional verbose transaction property values cannot be null; use ElectrumXJsonValue.NullValue.Instance.",
                        nameof(additionalProperties)));
            }
        }

        AdditionalProperties = new ReadOnlyDictionary<string, ElectrumXJsonValue>(additionalPropertiesCopy);
    }

    /// <summary>32-byte block hash encoded as 64 hexadecimal characters.</summary>
    public string? BlockHash { get; }

    /// <summary>Unix timestamp of the containing block.</summary>
    public long? BlockTime { get; }

    /// <summary>Number of confirmations, when the transaction is confirmed.</summary>
    public long? Confirmations { get; }

    /// <summary>32-byte transaction hash returned by the server, encoded as 64 hexadecimal characters.</summary>
    public string? Hash { get; }

    /// <summary>Serialized transaction bytes encoded as hexadecimal.</summary>
    public string? Hex { get; }

    /// <summary>Transaction lock-time value.</summary>
    public long? LockTime { get; }

    /// <summary>Serialized transaction size in bytes.</summary>
    public long? Size { get; }

    /// <summary>Unix timestamp associated with the transaction, when supplied by the server.</summary>
    public long? Time { get; }

    /// <summary>32-byte transaction identifier (txid) encoded as 64 hexadecimal characters.</summary>
    public string? TransactionId { get; }

    /// <summary>Transaction version.</summary>
    public long? Version { get; }

    /// <summary>Transaction inputs in wire order.</summary>
    public IReadOnlyList<ElectrumXVerboseTransactionInput>? Inputs { get; }

    /// <summary>Transaction outputs in wire order.</summary>
    public IReadOnlyList<ElectrumXVerboseTransactionOutput>? Outputs { get; }

    /// <summary>Additional coin-specific transaction fields not defined by the current model.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; }

    private static bool IsKnownProperty(string name)
    {
        return name is ElectrumXJsonPropertyNames.VerboseTransactionResult.BlockHash or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.BlockTime or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.Confirmations or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.Hash or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.Hex or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.LockTime or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.Size or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.Time or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.TransactionId or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.Version or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.Inputs or
            ElectrumXJsonPropertyNames.VerboseTransactionResult.Outputs;
    }
}
