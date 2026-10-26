namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;

/// <summary>Query parameters for server.version.</summary>
public sealed class ElectrumXServerVersionQuery(string clientName, string protocolMinimum, string protocolMaximum) : ElectrumXQuery
{
    /// <summary>Gets the client name sent during protocol negotiation.</summary>
    public string ClientName { get; } = clientName;

    /// <summary>Gets the minimum protocol version requested by the client.</summary>
    public string ProtocolMinimum { get; } = protocolMinimum;

    /// <summary>Gets the maximum protocol version requested by the client.</summary>
    public string ProtocolMaximum { get; } = protocolMaximum;
}
