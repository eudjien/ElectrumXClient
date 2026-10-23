using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

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
