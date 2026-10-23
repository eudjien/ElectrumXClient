using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;

/// <summary>The master nodes returned for the requested payees.</summary>
public sealed class ElectrumXMasterNodes : ElectrumXResult
{
    /// <summary>Creates a result containing masternode entries for the requested payees.</summary>
    public ElectrumXMasterNodes(IReadOnlyList<ElectrumXMasterNode> value)
    {
        Value = value;
    }

    /// <summary>Masternode records returned for the requested payee addresses.</summary>
    public IReadOnlyList<ElectrumXMasterNode> Value { get; }

    public static implicit operator ElectrumXMasterNode[](ElectrumXMasterNodes response)
    {
        return [.. response.Value];
    }
}
