using System;
using System.Net;

namespace NorthGomel.ElectrumXClient;

internal static class ArgumentGuard
{
    /// <summary>Trims and canonicalizes a host name or IP address, mapping wildcard addresses to loopback.</summary>
    /// <param name="host">The host name or IP address to normalize.</param>
    /// <param name="parameterName">The name of the argument represented by <paramref name="host" />.</param>
    /// <returns>A normalized host name or IP address suitable for connecting to.</returns>
    /// <exception cref="ArgumentException">The host is empty or is not a valid IP address or DNS host name.</exception>
    public static string NormalizeAndValidateHost(string? host, string parameterName)
    {
        Abstractions.ArgumentGuard.ThrowIfNullOrWhiteSpace(host, parameterName);

        host = host.Trim();

        if (IPAddress.TryParse(host, out var address))
        {
            if (address.Equals(IPAddress.Any))
            {
                return IPAddress.Loopback.ToString();
            }

            if (address.Equals(IPAddress.IPv6Any))
            {
                return IPAddress.IPv6Loopback.ToString();
            }

            return address.ToString();
        }

        if (Uri.CheckHostName(host) != UriHostNameType.Dns)
        {
            throw new ArgumentException("Host must be a valid IP address or DNS host name.", parameterName);
        }

        return host.ToLowerInvariant();
    }
}
