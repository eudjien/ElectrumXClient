using NorthGomel.ElectrumXClient.Abstractions;
using System.Net.WebSockets;

namespace NorthGomel.ElectrumXClient.JsonRpc.Clients;

/// <summary>Configures the options used by an ElectrumX WebSocket client.</summary>
/// <param name="options">The WebSocket options to configure.</param>
public delegate void ElectrumXWebSocketOptionsConfigurator(ClientWebSocketOptions options);

/// <summary>JSON-RPC transport that communicates over a TLS-encrypted WebSocket connection.</summary>
/// <remarks>
///     TLS-related settings supported by the target framework can be configured through
///     <see cref="ClientWebSocketOptions" />.
/// </remarks>
public sealed class ElectrumXTlsWebSocketRpcClient : ElectrumXWebSocketRpcClient
{
    /// <summary>Creates a TLS-encrypted WebSocket JSON-RPC client using the configured host and port.</summary>
    /// <param name="clientOptions">The ElectrumX RPC options.</param>
    /// <param name="configureWebSocketOptions">Optional callback to configure WebSocket and TLS options.</param>
    public ElectrumXTlsWebSocketRpcClient(
        IElectrumXRpcClientOptions clientOptions,
        ElectrumXWebSocketOptionsConfigurator? configureWebSocketOptions = null)
        : base(
            clientOptions,
            CreateEndpoint(clientOptions, "wss"),
            CreateOptionsConfigurator(configureWebSocketOptions))
    {
    }

    private static ElectrumXWebSocketOptionsConfigurator CreateOptionsConfigurator(
        ElectrumXWebSocketOptionsConfigurator? configureWebSocketOptions)
    {
        return options =>
        {
            options.RemoteCertificateValidationCallback =
                (_, _, _, _) => true;
            configureWebSocketOptions?.Invoke(options);
        };
    }
}
