using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>Defines the connection, request, and notification operations required by ElectrumXClient.</summary>
public interface IElectrumXRpcClient : IAsyncDisposable
{
    /// <summary>Options used to configure this JSON-RPC client.</summary>
    IElectrumXRpcClientOptions ClientOptions { get; }

    /// <summary>Negotiated server software and protocol version.</summary>
    ElectrumXServerVersion ServerVersion { get; }

    /// <summary>
    ///     Gets whether the connection is currently established and protocol negotiation has completed.
    ///     This is a point-in-time indication and does not guarantee that a subsequent RPC call will succeed.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>Raised when a typed block-header notification is received.</summary>
    event EventHandler<ElectrumXBlockHeaderEventArgs>? BlockHeaderReceived;

    /// <summary>Raised when a legacy block-height notification is received.</summary>
    event EventHandler<ElectrumXBlockHeightEventArgs>? BlockHeightReceived;

    /// <summary>Raised when a typed script-hash status notification is received.</summary>
    event EventHandler<ElectrumXScriptHashStatusEventArgs>? ScriptHashStatusChanged;

    /// <summary>Raised when a typed masterNode status notification is received.</summary>
    event EventHandler<ElectrumXMasterNodeStatusEventArgs>? MasterNodeStatusChanged;

    /// <summary>Raised when an address status notification is received.</summary>
    event EventHandler<ElectrumXAddressNotificationEventArgs>? AddressStatusChanged;

    /// <summary>Raised when a transaction notification is received.</summary>
    event EventHandler<ElectrumXTransactionNotificationEventArgs>? TransactionStatusChanged;

    /// <summary>Raised when a double-spend-proof notification is received.</summary>
    event EventHandler<ElectrumXDoubleSpendProofNotificationEventArgs>? DoubleSpendProofStatusChanged;

    /// <summary>Connects to the configured endpoint and negotiates the Electrum protocol version.</summary>
    /// <param name="cancellationToken">
    ///     Token used to cancel connection and negotiation. The configured connection timeout also
    ///     applies.
    /// </param>
    /// <returns>A task that completes when the connection and protocol negotiation succeed.</returns>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>Sends a JSON-RPC request and deserializes its data or error response.</summary>
    /// <typeparam name="TResult">Expected result type, derived from <see cref="ElectrumXResult" />.</typeparam>
    /// <typeparam name="TQuery">Query model associated with the request.</typeparam>
    /// <param name="request">Non-null JSON-RPC request envelope.</param>
    /// <param name="cancellationToken">Token used to cancel the request. The configured request timeout also applies.</param>
    /// <returns>The typed JSON-RPC response.</returns>
    Task<JsonRpcResponse<TResult>> CallAsync<TResult, TQuery>(
        JsonRpcRequest<TQuery> request,
        CancellationToken cancellationToken = default)
        where TResult : ElectrumXResult
        where TQuery : ElectrumXQuery;
}
