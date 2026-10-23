using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The unspent outputs returned for a script hash.</summary>
public sealed class ElectrumXUnspentOutputs : ElectrumXResult
{
    /// <summary>Creates a result containing a script hash's unspent outputs.</summary>
    public ElectrumXUnspentOutputs(IReadOnlyList<ElectrumXUnspentOutput> value)
    {
        Value = value;
    }

    /// <summary>Unspent outputs associated with the requested script hash.</summary>
    public IReadOnlyList<ElectrumXUnspentOutput> Value { get; }

    public static implicit operator ElectrumXUnspentOutput[](ElectrumXUnspentOutputs response)
    {
        return [.. response.Value];
    }
}
