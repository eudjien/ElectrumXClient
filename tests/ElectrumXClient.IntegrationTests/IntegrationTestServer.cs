using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.JsonRpc;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;
using System;

namespace NorthGomel.ElectrumXClient.IntegrationTests;

internal sealed record IntegrationTestServer(
    string Transport,
    string Host,
    int Port,
    Func<IElectrumXRpcClient> RpcClientFactory)
{
    internal static readonly IntegrationTestServer Tcp = Create(
        "TCP",
        50001,
        (host, port) => new ElectrumXTcpRpcClient(new ElectrumXRpcClientOptions
        {
            Host = host, Port = port, RequestTimeout = TimeSpan.FromMinutes(5)
        }));

    internal static readonly IntegrationTestServer Ssl = Create(
        "SSL",
        50002,
        (host, port) => new ElectrumXTlsTcpRpcClient(new ElectrumXRpcClientOptions
        {

            Host = host, Port = port, RequestTimeout = TimeSpan.FromMinutes(5)
        }));

    internal static readonly IntegrationTestServer WebSocket = Create(
        "WS",
        50003,
        (host, port) => new ElectrumXWebSocketRpcClient(new ElectrumXRpcClientOptions
        {
            Host = host, Port = port, RequestTimeout = TimeSpan.FromMinutes(5)
        }));

    internal static readonly IntegrationTestServer SslWebSocket = Create(
        "WSS",
        50004,
        (host, port) => new ElectrumXTlsWebSocketRpcClient(new ElectrumXRpcClientOptions
        {
            Host = host, Port = port, RequestTimeout = TimeSpan.FromMinutes(5)
        }));

    private static IntegrationTestServer Create(
        string transport,
        int port,
        Func<string, int, IElectrumXRpcClient> createClient)
    {
        const string host = "127.0.0.1";

        return new IntegrationTestServer(
            transport,
            host,
            port,
            () => createClient(host, port));
    }
}
