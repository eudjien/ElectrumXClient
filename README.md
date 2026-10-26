# ElectrumXClient

A .NET client for servers that implement the Electrum Cash Protocol, including compatible ElectrumX servers. It exposes typed blockchain, server, and mempool APIs over TCP, TLS, WebSocket, and TLS-over-WebSocket, with additional Dash masternode and ProTx APIs.

The library targets .NET Standard 2.1, which is supported by .NET Core 3.0 and later and .NET 5 and later (but not .NET Framework).

## What it can do

Use `ElectrumXClient` to access all API groups through one connection, or choose the dedicated clients for only the groups you need:

- `ElectrumXBlockchainClient` — headers, script-hash queries, utxos, transactions, and fees
- `ElectrumXMempoolClient` — mempool queries
- `ElectrumXServerClient` — server metadata and server-level RPC calls
- `ElectrumXMasterNodeClient` — masternode announcements, lists, subscriptions, and status notifications on compatible Dash-derived servers
- `ElectrumXProTxClient` — Dash ProTx diff and info queries on compatible servers

This library targets Bitcoin Cash and the Electrum Cash Protocol. Server-specific methods and Dash extensions are only available when supported by the connected server. It is designed for protocol-level integration, not for a higher-level app-specific wallet or business layer.

## Supported protocols

- TCP
- TLS over TCP
- WebSocket (WS)
- WebSocket Secure (WSS)

## Connecting clients

Each API client can connect directly over plain TCP with host and port. The examples use the conventional Electrum Cash Protocol ports; server configurations may differ:

```csharp
using NorthGomel.ElectrumXClient.Clients;

await using var blockchain = await ElectrumXBlockchainClient.ConnectAsync("electrum.example.com", 50001);
await using var mempool = await ElectrumXMempoolClient.ConnectAsync("electrum.example.com", 50001);
await using var server = await ElectrumXServerClient.ConnectAsync("electrum.example.com", 50001);

var version = blockchain.ServerVersion;
var histogram = await mempool.GetFeeHistogramAsync();
var features = await server.GetServerFeaturesAsync();
```

For TLS and WebSocket, construct the corresponding RPC transport directly and pass it to the desired client's
`ConnectAsync` method. Custom RPC implementations can be supplied the same way.

## Quick start

Use a dedicated API client for the group you need, or pass an already-created RPC transport directly into a client constructor.

### TCP connection

```csharp
using NorthGomel.ElectrumXClient.Clients;

await using var client = await ElectrumXBlockchainClient.ConnectAsync("127.0.0.1", 50001);

var version = client.ServerVersion;
Console.WriteLine($"Connected: {version.SoftwareVersion}");
```

### TLS over TCP

```csharp
using NorthGomel.ElectrumXClient.Clients;
using NorthGomel.ElectrumXClient.JsonRpc;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;

var rpcClient = new ElectrumXTlsTcpRpcClient(new ElectrumXRpcClientOptions
{
    Host = "electrum.example.com",
    Port = 50002,
    ClientName = "MyElectrumApp"
});

await using var client = await ElectrumXBlockchainClient.ConnectAsync(rpcClient);
```

### TLS over WebSocket

```csharp
using NorthGomel.ElectrumXClient.Clients;
using NorthGomel.ElectrumXClient.JsonRpc;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;

var rpcClient = new ElectrumXTlsWebSocketRpcClient(new ElectrumXRpcClientOptions
{
    Host = "electrum.example.com",
    Port = 50004,
    ClientName = "MyElectrumApp"
});

await using var client = await ElectrumXBlockchainClient.ConnectAsync(rpcClient);
```

### TLS certificate validation

TLS transports let you configure certificate validation. The following .NET 5+ example trusts a private CA certificate in addition to checking that the server certificate is valid for the requested host:

```csharp
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using NorthGomel.ElectrumXClient.Clients;
using NorthGomel.ElectrumXClient.JsonRpc;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;

using var trustedRoot = X509Certificate2.CreateFromPemFile("certs/electrum-root-ca.pem");

bool ValidateServerCertificate(
    X509Certificate? certificate,
    X509Chain? chain,
    SslPolicyErrors errors)
{
    if (certificate is not X509Certificate2 serverCertificate ||
        (errors & (SslPolicyErrors.RemoteCertificateNameMismatch |
                   SslPolicyErrors.RemoteCertificateNotAvailable)) != SslPolicyErrors.None)
    {
        return false;
    }

    using var customChain = new X509Chain();
    customChain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
    customChain.ChainPolicy.CustomTrustStore.Add(trustedRoot);

    return customChain.Build(serverCertificate);
}

var rpcClient = new ElectrumXTlsTcpRpcClient(
    new ElectrumXRpcClientOptions
    {
        Host = "electrum.example.com",
        Port = 50002
    },
    (authenticationOptions, _) =>
    {
        authenticationOptions.RemoteCertificateValidationCallback =
            (_, certificate, chain, errors) => ValidateServerCertificate(certificate, chain, errors);
    });

await using var client = await ElectrumXBlockchainClient.ConnectAsync(rpcClient);
```

For TLS over WebSocket, configure the equivalent callback on `ClientWebSocketOptions`:

```csharp
var rpcClient = new ElectrumXTlsWebSocketRpcClient(
    new ElectrumXRpcClientOptions
    {
        Host = "electrum.example.com",
        Port = 50004
    },
    options =>
    {
        options.RemoteCertificateValidationCallback =
            (_, certificate, chain, errors) => ValidateServerCertificate(certificate, chain, errors);
    });
```

If the certificate chains to a CA already trusted by the operating system, validate it against the system trust store instead. For TLS over TCP:

```csharp
var rpcClient = new ElectrumXTlsTcpRpcClient(
    new ElectrumXRpcClientOptions
    {
        Host = "electrum.example.com",
        Port = 50002
    },
    (authenticationOptions, _) =>
    {
        authenticationOptions.RemoteCertificateValidationCallback =
            (_, _, _, errors) => errors == SslPolicyErrors.None;
    });
```

For TLS over WebSocket:

```csharp
var rpcClient = new ElectrumXTlsWebSocketRpcClient(
    new ElectrumXRpcClientOptions
    {
        Host = "electrum.example.com",
        Port = 50004
    },
    options =>
    {
        options.RemoteCertificateValidationCallback =
            (_, _, _, errors) => errors == SslPolicyErrors.None;
    });
```

The TLS transports currently accept all certificates unless you configure a validation callback. Do not accept every certificate: that disables server authentication and makes the connection vulnerable to interception. Keep a custom CA certificate file protected and load it from a trusted location.

### WebSocket

```csharp
using NorthGomel.ElectrumXClient.Clients;
using NorthGomel.ElectrumXClient.JsonRpc;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;

var rpcClient = new ElectrumXWebSocketRpcClient(new ElectrumXRpcClientOptions
{
    Host = "127.0.0.1",
    Port = 50003,
    ClientName = "MyElectrumApp"
});

await using var client = await ElectrumXBlockchainClient.ConnectAsync(rpcClient);
```

## Example RPC requests

### blockchain.scripthash.get_balance

```csharp
using NorthGomel.ElectrumXClient.Clients;

await using var client = await ElectrumXBlockchainClient.ConnectAsync("127.0.0.1", 50001);

var scriptHash = "2b132844f5e06a9ada2cccce4dffe6a8194202b1938610a74f245eee3487adf4";
var balance = await client.GetBalanceAsync(scriptHash);

Console.WriteLine($"Confirmed: {balance.Confirmed}");
Console.WriteLine($"Unconfirmed: {balance.Unconfirmed}");
```

### blockchain.header.get

```csharp
using NorthGomel.ElectrumXClient.Clients;

await using var client = await ElectrumXBlockchainClient.ConnectAsync("127.0.0.1", 50001);

var header = await client.GetHeaderAsync(0);

Console.WriteLine(header.Height);
Console.WriteLine(header.Hex);
```

### blockchain.address.get_history

```csharp
using NorthGomel.ElectrumXClient.Clients;

await using var client = await ElectrumXBlockchainClient.ConnectAsync("127.0.0.1", 50001);

var scriptHash = "2b132844f5e06a9ada2cccce4dffe6a8194202b1938610a74f245eee3487adf4";
var history = await client.GetHistoryAsync(scriptHash);

Console.WriteLine(history.Count);
foreach (var item in history)
{
    Console.WriteLine($"{item.TxHash} : {item.Height}");
}
```

### mempool.get_info

```csharp
using NorthGomel.ElectrumXClient.Clients;

await using var mempool = await ElectrumXMempoolClient.ConnectAsync("127.0.0.1", 50001);

var info = await mempool.GetInfoAsync();
Console.WriteLine($"Height: {info.Height}");
Console.WriteLine($"Fee histogram entries: {info.FeeHistogram.Count}");
```

## Important nuances

- Protocol features vary by Electrum Cash Protocol and server version.
- Not every RPC method is available on every server; the library validates method support and protocol version ranges.
- Notifications are first-class and suitable for real-time monitoring.
- The client is asynchronous and should be disposed with `await using`.
- The library is protocol-aware; you still need to understand ElectrumX server capabilities.

See [protocol method versions](docs/protocol-method-versions.md) for method availability, deprecation, and removal history.

## Summary

ElectrumXClient is a .NET library for connecting to Electrum Cash Protocol servers, issuing typed RPC calls, and receiving server notifications. It is best used when you want a low-level client and are comfortable working with the protocol directly.

> This library has not been thoroughly tested. Carefully validate it in your own environment before relying on it in production.
