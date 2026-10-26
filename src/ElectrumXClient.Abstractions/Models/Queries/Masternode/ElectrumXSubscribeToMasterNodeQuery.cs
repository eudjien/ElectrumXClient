namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Masternode;

/// <summary>Query parameters for masternode.subscribe.</summary>
public sealed class ElectrumXSubscribeToMasterNodeQuery : ElectrumXQuery
{
    /// <summary>Creates a query that subscribes to updates for a masternode collateral.</summary>
    /// <param name="collateral">Masternode collateral identifier.</param>
    public ElectrumXSubscribeToMasterNodeQuery(string collateral)
    {
        ArgumentGuard.ThrowIfNullOrWhiteSpace(collateral, nameof(collateral));
        Collateral = collateral;
    }

    /// <summary>Gets the masternode collateral identifier to monitor.</summary>
    public string Collateral { get; }
}
