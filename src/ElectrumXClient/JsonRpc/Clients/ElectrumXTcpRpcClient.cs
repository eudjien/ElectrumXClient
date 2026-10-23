using NorthGomel.ElectrumXClient.Abstractions;

namespace NorthGomel.ElectrumXClient.JsonRpc.Clients;

/// <summary>JSON-RPC transport that communicates over an unencrypted TCP connection.</summary>
public class ElectrumXTcpRpcClient : ElectrumXTcpRpcClientBase
{
    /// <summary>Creates a plain TCP JSON-RPC client using the default TCP implementation.</summary>
    public ElectrumXTcpRpcClient(IElectrumXRpcClientOptions clientOptions)
        : base(clientOptions)
    {
    }
}
