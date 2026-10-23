using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a blockchain.transaction.dsproof.subscribe notification.</summary>
public sealed class ElectrumXDoubleSpendProofNotification(
    string transactionHash,
    ElectrumXDoubleSpendProof? doubleSpendProof) : ElectrumXNotification
{
    /// <summary>The 32-byte transaction hash encoded as hexadecimal.</summary>
    public string TransactionHash { get; } = transactionHash;

    /// <summary>The associated double-spend proof, or <see langword="null" /> when no proof is available.</summary>
    public ElectrumXDoubleSpendProof? DoubleSpendProof { get; } = doubleSpendProof;
}
