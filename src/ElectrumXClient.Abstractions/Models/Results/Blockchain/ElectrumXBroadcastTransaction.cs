namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The transaction ID returned after broadcasting a transaction.</summary>
public sealed class ElectrumXBroadcastTransaction : ElectrumXResult
{
    /// <summary>Creates a result containing the transaction hash returned by the server.</summary>
    public ElectrumXBroadcastTransaction(string value)
    {
        Value = value;
    }

    /// <summary>32-byte transaction hash returned by <c>blockchain.transaction.broadcast</c>, encoded as 64 hexadecimal characters.</summary>
    public string Value { get; }

    public static implicit operator string(ElectrumXBroadcastTransaction response)
    {
        return response.Value;
    }
}
