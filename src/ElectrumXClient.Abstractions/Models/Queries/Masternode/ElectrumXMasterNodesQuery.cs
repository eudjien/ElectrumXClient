using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.MasterNode;

/// <summary>Query parameters for masternode.list.</summary>
public sealed class ElectrumXMasterNodesQuery : ElectrumXQuery
{
    public ElectrumXMasterNodesQuery(IReadOnlyCollection<string> payees)
    {
        ArgumentGuard.ThrowIfNull(payees, nameof(payees));

        foreach (string payee in payees)
        {
            ArgumentGuard.ThrowIfNullOrWhiteSpace(payee, nameof(payees));
        }

        Payees = payees;
    }

    public IReadOnlyCollection<string> Payees { get; }
}
