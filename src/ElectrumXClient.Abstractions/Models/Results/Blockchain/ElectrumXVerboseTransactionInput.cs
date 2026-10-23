using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

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
