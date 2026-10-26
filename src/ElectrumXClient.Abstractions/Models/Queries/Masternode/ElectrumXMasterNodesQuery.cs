using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Masternode;

/// <summary>Query parameters for masternode.list.</summary>
public sealed class ElectrumXMasterNodesQuery : ElectrumXQuery
{
    /// <summary>Creates a query that filters masternodes by payee address.</summary>
    /// <param name="payees">Payee addresses used to filter the masternode list.</param>
    public ElectrumXMasterNodesQuery(IReadOnlyCollection<string> payees)
    {
        ArgumentGuard.ThrowIfNull(payees, nameof(payees));

        foreach (string payee in payees)
        {
            ArgumentGuard.ThrowIfNullOrWhiteSpace(payee, nameof(payees));
        }

        Payees = payees;
    }

    /// <summary>Gets the payee addresses used to filter the masternode list.</summary>
    public IReadOnlyCollection<string> Payees { get; }
}
