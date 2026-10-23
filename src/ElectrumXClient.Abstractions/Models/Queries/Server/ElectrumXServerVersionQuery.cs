namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;

/// <summary>Query parameters for server.version.</summary>
public sealed class ElectrumXServerVersionQuery(string clientName, string protocolMinimum, string protocolMaximum) : ElectrumXQuery
{
    public string ClientName { get; } = clientName;

    public string ProtocolMinimum { get; } = protocolMinimum;

    public string ProtocolMaximum { get; } = protocolMaximum;
}
