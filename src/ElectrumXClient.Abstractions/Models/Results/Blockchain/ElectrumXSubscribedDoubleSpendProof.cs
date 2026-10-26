using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The proof returned by blockchain.transaction.dsproof.subscribe.</summary>
public sealed class ElectrumXSubscribedDoubleSpendProof(
    string doubleSpendProofId,
    string transactionId,
    string hex,
    ElectrumXDoubleSpendProofOutpoint outpoint,
    IReadOnlyList<string> descendants) : ElectrumXResult
{
    /// <summary>32-byte double-spend proof identifier encoded as 64 hexadecimal characters.</summary>
    public string DoubleSpendProofId { get; } = doubleSpendProofId;

    /// <summary>32-byte transaction hash associated with the proof, encoded as 64 hexadecimal characters.</summary>
    public string TransactionId { get; } = transactionId;

    /// <summary>Serialized double-spend proof bytes encoded as hexadecimal.</summary>
    public string Hex { get; } = hex;

    /// <summary>The outpoint spent by the conflicting transactions.</summary>
    public ElectrumXDoubleSpendProofOutpoint Outpoint { get; } = outpoint;

    /// <summary>32-byte descendant transaction hashes encoded as 64 hexadecimal characters.</summary>
    public IReadOnlyList<string> Descendants { get; } = descendants;
}
