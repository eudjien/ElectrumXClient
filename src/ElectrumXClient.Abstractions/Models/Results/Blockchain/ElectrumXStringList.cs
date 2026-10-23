using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A list of strings returned by a blockchain RPC method.</summary>
public sealed class ElectrumXStringList(IReadOnlyList<string> value) : ElectrumXResult
{
    /// <summary>The returned strings.</summary>
    public IReadOnlyList<string> Value { get; } = value;
}
