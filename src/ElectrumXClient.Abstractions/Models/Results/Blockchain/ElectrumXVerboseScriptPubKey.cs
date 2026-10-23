using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

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
