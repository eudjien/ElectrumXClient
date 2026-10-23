using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Clients;

/// <summary>Provides typed ElectrumX protocol operations and server notifications.</summary>
public interface IElectrumXClient : IAsyncDisposable
{
    /// <summary>Blockchain-related operations.</summary>
    IElectrumXBlockchainClient Blockchain { get; }

    /// <summary>Server-related operations.</summary>
    IElectrumXServerClient Server { get; }

    /// <summary>Masternode-related operations.</summary>
    IElectrumXMasterNodeClient MasterNode { get; }

    /// <summary>ProTx-related operations.</summary>
    IElectrumXProTxClient ProTx { get; }

    /// <summary>Mempool-related operations.</summary>
    IElectrumXMempoolClient Mempool { get; }

    /// <summary>Server software and negotiated protocol version.</summary>
    /// <remarks>Negotiated using JSON-RPC method <c>server.version</c>.</remarks>
    ElectrumXServerVersion ServerVersion { get; }
}
