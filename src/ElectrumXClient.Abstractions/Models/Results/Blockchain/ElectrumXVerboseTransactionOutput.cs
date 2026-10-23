using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

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
