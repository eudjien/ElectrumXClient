using NorthGomel.ElectrumXClient.Abstractions;

namespace NorthGomel.ElectrumXClient.Clients;

internal static class ElectrumXProtocolVersions
{
    internal static readonly ElectrumXProtocolVersion Version100 = new(1, 0);
    internal static readonly ElectrumXProtocolVersion Version110 = new(1, 1);
    internal static readonly ElectrumXProtocolVersion Version120 = new(1, 2);
    internal static readonly ElectrumXProtocolVersion Version130 = new(1, 3);
    internal static readonly ElectrumXProtocolVersion Version140 = new(1, 4);
    internal static readonly ElectrumXProtocolVersion Version142 = new(1, 4, 2);
    internal static readonly ElectrumXProtocolVersion Version143 = new(1, 4, 3);
    internal static readonly ElectrumXProtocolVersion Version144 = new(1, 4, 4);
    internal static readonly ElectrumXProtocolVersion Version145 = new(1, 4, 5);
    internal static readonly ElectrumXProtocolVersion Version150 = new(1, 5, 0);
    internal static readonly ElectrumXProtocolVersion Version151 = new(1, 5, 1);
    internal static readonly ElectrumXProtocolVersion Version152 = new(1, 5, 2);
    internal static readonly ElectrumXProtocolVersion Version153 = new(1, 5, 3);
    internal static readonly ElectrumXProtocolVersion Version160 = new(1, 6, 0);
}
