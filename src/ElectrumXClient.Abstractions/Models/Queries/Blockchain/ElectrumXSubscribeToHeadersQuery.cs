namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.headers.subscribe.</summary>
public sealed class ElectrumXSubscribeToHeadersQuery(bool? raw = null) : ElectrumXQuery
{
    /// <summary>Whether to request a raw header in protocol versions that support this parameter.</summary>
    public bool? Raw { get; } = raw;
}
