using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Converters;
using System;
using System.Net;

namespace NorthGomel.ElectrumXClient.JsonRpc;

/// <summary>Configuration shared by ElectrumX JSON-RPC transports.</summary>
public sealed class ElectrumXRpcClientOptions : IElectrumXRpcClientOptions
{
    private readonly string _clientName = "ElectrumXClient";
    private readonly TimeSpan _connectTimeout = TimeSpan.FromSeconds(15);
    private readonly IElectrumXJsonConverter _converter = new SystemTextJsonElectrumXJsonConverter();
    private readonly string _host = string.Empty;
    private readonly int _port;
    private readonly ElectrumXProtocolVersionLimits _protocolVersionLimits = new();
    private readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(30);

    /// <inheritdoc />
    public IElectrumXJsonConverter Converter
    {
        get => _converter;
        init => _converter = value ?? throw new ArgumentNullException(nameof(Converter));
    }

    /// <inheritdoc />
    public string Host
    {
        get => _host;
        init => _host = ArgumentGuard.NormalizeAndValidateHost(value, nameof(Host));
    }

    /// <inheritdoc />
    public int Port
    {
        get => _port;
        init
        {
            if (value is < 1 or > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(nameof(Port), $"Port must be between 1 and {IPEndPoint.MaxPort}.");
            }

            _port = value;
        }
    }

    /// <inheritdoc />
    public string ClientName
    {
        get => _clientName;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(ClientName));
            }

            _clientName = value.Trim();
        }
    }

    /// <inheritdoc />
    public ElectrumXProtocolVersionLimits ProtocolVersions
    {
        get => _protocolVersionLimits;
        init => _protocolVersionLimits = value ?? throw new ArgumentNullException(nameof(ProtocolVersions));
    }

    /// <inheritdoc />
    public TimeSpan ConnectTimeout
    {
        get => _connectTimeout;
        init
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(ConnectTimeout), value, "The timeout must be greater than zero.");
            }

            _connectTimeout = value;
        }
    }

    /// <inheritdoc />
    public TimeSpan RequestTimeout
    {
        get => _requestTimeout;
        init
        {
            if (value <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(RequestTimeout), value, "The timeout must be greater than zero.");
            }

            _requestTimeout = value;
        }
    }
}
