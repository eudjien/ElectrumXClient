using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;

/// <summary>Query parameters for daemon.passthrough.</summary>
public sealed class ElectrumXDaemonPassthroughQuery : ElectrumXQuery
{
    /// <summary>Creates a daemon RPC passthrough query.</summary>
    public ElectrumXDaemonPassthroughQuery(string method, IReadOnlyList<ElectrumXJsonValue>? parameters = null)
    {
        ArgumentGuard.ThrowIfNull(method, nameof(method));
        Method = method;
        Parameters = parameters ?? [];
    }

    /// <summary>The backing daemon RPC method.</summary>
    public string Method { get; }

    /// <summary>Positional arguments forwarded to the backing daemon.</summary>
    public IReadOnlyList<ElectrumXJsonValue> Parameters { get; }
}
