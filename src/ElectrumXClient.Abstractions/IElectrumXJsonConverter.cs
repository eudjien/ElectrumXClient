using NorthGomel.ElectrumXClient.Abstractions.Exceptions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using System;

namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>Converts ElectrumX RPC values to and from JSON.</summary>
/// <remarks>
///     Serializer exceptions are wrapped in <see cref="ElectrumXProtocolException" /> when the converter is used by
///     the ElectrumX client to process protocol messages.
/// </remarks>
public interface IElectrumXJsonConverter
{
    /// <summary>Serializes a JSON-RPC request envelope.</summary>
    byte[] SerializeRequest<TQuery>(JsonRpcRequest<TQuery> request)
        where TQuery : ElectrumXQuery;

    /// <summary>Deserializes a JSON-RPC response into its concrete data or error model.</summary>
    JsonRpcResponse<TResponse> DeserializeResponse<TResponse>(ReadOnlyMemory<byte> json)
        where TResponse : ElectrumXResult;

    /// <summary>Deserializes a JSON-RPC notification into its concrete parameter model.</summary>
    JsonRpcNotification<TParams> DeserializeNotification<TParams>(ReadOnlyMemory<byte> json)
        where TParams : ElectrumXNotification;

    /// <summary>Reads the JSON-RPC message kind and its routing fields.</summary>
    JsonRpcHeader DeserializeHeader(ReadOnlyMemory<byte> json);
}
