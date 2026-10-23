using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;

/// <summary>Proof-backed deterministic masternode list changes between two heights.</summary>
public sealed class ElectrumXProTxDiff : ElectrumXResult
{
    /// <summary>Creates a deterministic masternode-list diff with its associated proof data.</summary>
    public ElectrumXProTxDiff(string? baseBlockHash = null, string? blockHash = null, string? coinbaseTransactionMerkleTree = null,
        string? coinbaseTransaction = null, IReadOnlyList<string>? deletedMasterNodes = null, IReadOnlyList<ElectrumXProTxMasterNode>? masterNodes = null,
        string? masterNodeListMerkleRoot = null)
    {
        BaseBlockHash = baseBlockHash;
        BlockHash = blockHash;
        CoinbaseTransactionMerkleTree = coinbaseTransactionMerkleTree;
        CoinbaseTransaction = coinbaseTransaction;
        DeletedMasterNodes = deletedMasterNodes;
        MasterNodes = masterNodes;
        MasterNodeListMerkleRoot = masterNodeListMerkleRoot;
    }

    /// <summary>32-byte hash of the block at the base height, encoded as 64 hexadecimal characters.</summary>
    public string? BaseBlockHash { get; }

    /// <summary>32-byte hash of the block at the requested height, encoded as 64 hexadecimal characters.</summary>
    public string? BlockHash { get; }

    /// <summary>Serialized Merkle-tree proof bytes encoded as hexadecimal, when supplied by the server.</summary>
    public string? CoinbaseTransactionMerkleTree { get; }

    /// <summary>Serialized coinbase transaction bytes encoded as hexadecimal, when supplied by the server.</summary>
    public string? CoinbaseTransaction { get; }

    /// <summary>32-byte ProRegTx hashes removed from the deterministic masternode list, each encoded as 64 hexadecimal characters.</summary>
    public IReadOnlyList<string>? DeletedMasterNodes { get; }

    /// <summary>Masternode records included in the list diff.</summary>
    public IReadOnlyList<ElectrumXProTxMasterNode>? MasterNodes { get; }

    /// <summary>32-byte Merkle root of the deterministic masternode list, encoded as 64 hexadecimal characters.</summary>
    public string? MasterNodeListMerkleRoot { get; }
}
