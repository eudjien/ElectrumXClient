namespace NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;

/// <summary>Parameters sent with a blockchain.transaction.subscribe notification.</summary>
public sealed class ElectrumXTransactionNotification(string transactionHash, long? height) : ElectrumXNotification
{
    /// <summary>The 32-byte transaction hash encoded as hexadecimal.</summary>
    public string TransactionHash { get; } = transactionHash;

    /// <summary>The confirmation height, zero for a mempool transaction, or <see langword="null" /> if unknown.</summary>
    public long? Height { get; } = height;
}
