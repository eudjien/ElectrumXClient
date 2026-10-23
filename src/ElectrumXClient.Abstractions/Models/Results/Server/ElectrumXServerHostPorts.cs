namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>TCP, SSL, and WebSocket ports advertised for an ElectrumX server host.</summary>
public sealed class ElectrumXServerHostPorts(
    int? sslPort,
    int? tcpPort,
    int? webSocketPort = null,
    int? webSocketSecurePort = null)
{
    /// <summary>SSL port, or <see langword="null" /> when unavailable.</summary>
    public int? SslPort { get; } = sslPort;

    /// <summary>TCP port, or <see langword="null" /> when unavailable.</summary>
    public int? TcpPort { get; } = tcpPort;

    /// <summary>WebSocket port, or <see langword="null" /> when unavailable.</summary>
    public int? WebSocketPort { get; } = webSocketPort;

    /// <summary>Secure WebSocket port, or <see langword="null" /> when unavailable.</summary>
    public int? WebSocketSecurePort { get; } = webSocketSecurePort;
}
