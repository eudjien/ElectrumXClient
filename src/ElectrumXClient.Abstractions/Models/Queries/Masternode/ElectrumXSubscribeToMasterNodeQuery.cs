namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.MasterNode;

/// <summary>Query parameters for masternode.subscribe.</summary>
public sealed class ElectrumXSubscribeToMasterNodeQuery : ElectrumXQuery
{
    public ElectrumXSubscribeToMasterNodeQuery(string collateral)
    {
        ArgumentGuard.ThrowIfNullOrWhiteSpace(collateral, nameof(collateral));
        Collateral = collateral;
    }

    public string Collateral { get; }
}
