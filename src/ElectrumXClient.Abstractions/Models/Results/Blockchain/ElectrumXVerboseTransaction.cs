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
                additionalPropertiesCopy[name] = value;
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
}

/// <summary>A verbose transaction input.</summary>
public sealed class ElectrumXVerboseTransactionInput : ElectrumXResult
{
    /// <summary>Creates a coin-specific verbose transaction input.</summary>
    public ElectrumXVerboseTransactionInput(string? transactionId = null, long? outputIndex = null, ElectrumXVerboseScriptSig? scriptSignature = null,
        ulong? sequence = null, string? coinbase = null)
        : this(transactionId, outputIndex, scriptSignature, sequence, coinbase, null)
    {
    }

    /// <summary>Creates a verbose transaction input with additional coin-specific properties.</summary>
    public ElectrumXVerboseTransactionInput(string? transactionId, long? outputIndex, ElectrumXVerboseScriptSig? scriptSignature,
        ulong? sequence, string? coinbase, IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties)
    {
        TransactionId = transactionId;
        OutputIndex = outputIndex;
        ScriptSignature = scriptSignature;
        Sequence = sequence;
        Coinbase = coinbase;
        AdditionalProperties = AdditionalPropertiesHelper.Copy(additionalProperties, IsKnownProperty);
    }

    /// <summary>32-byte hash of the previous transaction, encoded as 64 hexadecimal characters.</summary>
    public string? TransactionId { get; }

    /// <summary>Index of the referenced output in the previous transaction.</summary>
    public long? OutputIndex { get; }

    /// <summary>Unlocking script details for the input.</summary>
    public ElectrumXVerboseScriptSig? ScriptSignature { get; }

    /// <summary>Sequence number of the input.</summary>
    public ulong? Sequence { get; }

    /// <summary>Coinbase input script bytes encoded as hexadecimal, present instead of a previous transaction reference.</summary>
    public string? Coinbase { get; }

    /// <summary>Additional coin-specific transaction input fields not defined by the current model.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; }

    private static bool IsKnownProperty(string name)
    {
        return name is ElectrumXJsonPropertyNames.VerboseTransactionInputResult.TransactionId or
            ElectrumXJsonPropertyNames.VerboseTransactionInputResult.OutputIndex or
            ElectrumXJsonPropertyNames.VerboseTransactionInputResult.ScriptSignature or
            ElectrumXJsonPropertyNames.VerboseTransactionInputResult.Sequence or
            ElectrumXJsonPropertyNames.VerboseTransactionInputResult.Coinbase;
    }
}

/// <summary>Unlocking script details included in a verbose transaction input.</summary>
public sealed class ElectrumXVerboseScriptSig : ElectrumXResult
{
    /// <summary>Creates scriptSig details returned by the server.</summary>
    public ElectrumXVerboseScriptSig(string? assembly = null, string? hex = null)
        : this(assembly, hex, null)
    {
    }

    /// <summary>Creates scriptSig details with additional coin-specific fields.</summary>
    public ElectrumXVerboseScriptSig(
        string? assembly,
        string? hex,
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties)
    {
        Assembly = assembly;
        Hex = hex;
        AdditionalProperties = AdditionalPropertiesHelper.Copy(additionalProperties, IsKnownProperty);
    }

    /// <summary>Human-readable disassembly of the unlocking script, when provided by the server.</summary>
    public string? Assembly { get; }

    /// <summary>Unlocking-script bytes encoded as hexadecimal.</summary>
    public string? Hex { get; }

    /// <summary>Additional coin-specific scriptSig fields not defined by the current model.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; }

    private static bool IsKnownProperty(string name)
    {
        return name is ElectrumXJsonPropertyNames.VerboseScriptSigResult.Assembly or
            ElectrumXJsonPropertyNames.VerboseScriptSigResult.Hex;
    }
}

/// <summary>A verbose transaction output.</summary>
public sealed class ElectrumXVerboseTransactionOutput : ElectrumXResult
{
    /// <summary>Creates a coin-specific verbose transaction output.</summary>
    public ElectrumXVerboseTransactionOutput(long? index = null, ElectrumXVerboseScriptPubKey? scriptPubKey = null, decimal? value = null)
        : this(index, scriptPubKey, value, null)
    {
    }

    /// <summary>Creates a verbose transaction output with additional coin-specific properties.</summary>
    public ElectrumXVerboseTransactionOutput(long? index, ElectrumXVerboseScriptPubKey? scriptPubKey, decimal? value,
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties)
    {
        Index = index;
        ScriptPubKey = scriptPubKey;
        Value = value;
        AdditionalProperties = AdditionalPropertiesHelper.Copy(additionalProperties, IsKnownProperty);
    }

    /// <summary>Zero-based output index in the transaction.</summary>
    public long? Index { get; }

    /// <summary>Locking script details for the output.</summary>
    public ElectrumXVerboseScriptPubKey? ScriptPubKey { get; }

    /// <summary>Output amount in whole coin units.</summary>
    public decimal? Value { get; }

    /// <summary>Additional coin-specific transaction output fields not defined by the current model.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; }

    private static bool IsKnownProperty(string name)
    {
        return name is ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.Index or
            ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.ScriptPubKey or
            ElectrumXJsonPropertyNames.VerboseTransactionOutputResult.Value;
    }
}

/// <summary>Locking script details included in a verbose transaction output.</summary>
public sealed class ElectrumXVerboseScriptPubKey : ElectrumXResult
{
    /// <summary>Creates scriptPubKey details returned by the server.</summary>
    public ElectrumXVerboseScriptPubKey(string? assembly = null, string? hex = null, int? requiredSignatures = null, string? type = null,
        IReadOnlyList<string>? addresses = null)
        : this(assembly, hex, requiredSignatures, type, addresses, null)
    {
    }

    /// <summary>Creates scriptPubKey details with additional coin-specific fields.</summary>
    public ElectrumXVerboseScriptPubKey(string? assembly, string? hex, int? requiredSignatures, string? type,
        IReadOnlyList<string>? addresses, IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties)
    {
        Assembly = assembly;
        Hex = hex;
        RequiredSignatures = requiredSignatures;
        Type = type;
        Addresses = addresses;
        AdditionalProperties = AdditionalPropertiesHelper.Copy(additionalProperties, IsKnownProperty);
    }

    /// <summary>Human-readable disassembly of the locking script, when provided by the server.</summary>
    public string? Assembly { get; }

    /// <summary>Locking-script bytes encoded as hexadecimal.</summary>
    public string? Hex { get; }

    /// <summary>Number of signatures required to spend the output, when applicable.</summary>
    public int? RequiredSignatures { get; }

    /// <summary>Server-reported script type.</summary>
    public string? Type { get; }

    /// <summary>Chain-specific address strings associated with the script, when the server provides them.</summary>
    public IReadOnlyList<string>? Addresses { get; }

    /// <summary>Additional coin-specific scriptPubKey fields not defined by the current model.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; }

    private static bool IsKnownProperty(string name)
    {
        return name is ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Assembly or
            ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Hex or
            ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.RequiredSignatures or
            ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Type or
            ElectrumXJsonPropertyNames.VerboseScriptPubKeyResult.Addresses;
    }
}
