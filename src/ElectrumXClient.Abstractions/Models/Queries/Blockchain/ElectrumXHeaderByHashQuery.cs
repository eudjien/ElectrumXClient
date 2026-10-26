namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for retrieving a block header by hash.</summary>
public sealed class ElectrumXHeaderByHashQuery(string blockHash)
    : ElectrumXQuery
{
    /// <summary>Gets the hash of the requested block.</summary>
    public string BlockHash { get; } = blockHash;
}
