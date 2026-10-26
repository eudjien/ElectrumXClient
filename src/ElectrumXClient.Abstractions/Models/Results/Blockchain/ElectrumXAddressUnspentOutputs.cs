using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The unspent outputs returned by blockchain.address.listunspent.</summary>
public sealed class ElectrumXAddressUnspentOutputs(IReadOnlyList<ElectrumXUnspentOutput> value)
    : ElectrumXResult
{
    /// <summary>Unspent outputs associated with the requested address.</summary>
    public IReadOnlyList<ElectrumXUnspentOutput> Value { get; } = value;

    /// <summary>Converts the response to its unspent outputs.</summary>
    /// <param name="response">The address unspent-output response.</param>
    /// <returns>The contained unspent outputs.</returns>
    public static implicit operator ElectrumXUnspentOutput[](ElectrumXAddressUnspentOutputs response)
    {
        return [.. response.Value];
    }
}
