namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.id_from_pos.</summary>
public sealed class ElectrumXTransactionIdFromPositionQuery : ElectrumXQuery
{
    public ElectrumXTransactionIdFromPositionQuery(
        long height,
        int transactionPosition,
        bool merkle = ElectrumXJsonMethodArgumentDefaultValues.ElectrumXTransactionIdFromPositionQuery.Merkle)
    {
        ArgumentGuard.ThrowIfNegative(height, nameof(height));
        ArgumentGuard.ThrowIfNegative(transactionPosition, nameof(transactionPosition));

        Height = height;
        TransactionPosition = transactionPosition;
        Merkle = merkle;
    }

    public long Height { get; }

    public int TransactionPosition { get; }

    public bool Merkle { get; }
}
