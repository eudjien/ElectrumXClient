using System;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.JsonRpc;

internal interface IElectrumXRpcRequestSender
{
    ValueTask SendAsync(ReadOnlyMemory<byte> payload, CancellationToken cancellationToken);
}
