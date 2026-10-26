namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for blockchain.transaction.broadcast.</summary>
public sealed class ElectrumXBroadcastTransactionQuery(string rawTransaction) : ElectrumXQuery
{
    /// <summary>Gets the raw transaction serialized as hexadecimal.</summary>
    public string RawTransaction { get; } = ArgumentGuard.ValidateAndNormalizeHex(rawTransaction, nameof(rawTransaction));
}
