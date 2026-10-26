namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.id_from_pos.</summary>
public sealed class ElectrumXTransactionIdFromPositionQuery : ElectrumXQuery
{
    /// <summary>Creates a query for the transaction at a block position.</summary>
    /// <param name="height">Zero-based block height.</param>
    /// <param name="transactionPosition">Zero-based transaction position within the block.</param>
    /// <param name="merkle">Whether to include the transaction's Merkle proof.</param>
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

    /// <summary>Gets the block height.</summary>
    public long Height { get; }

    /// <summary>Gets the zero-based transaction position within the block.</summary>
    public int TransactionPosition { get; }

    /// <summary>Gets whether the response should include a Merkle proof.</summary>
    public bool Merkle { get; }
}
