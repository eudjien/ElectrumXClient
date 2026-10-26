namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.ProTx;

/// <summary>Query parameters for protx.info.</summary>
public sealed class ElectrumXProTxInfoQuery(string proTxHash) : ElectrumXQuery
{
    /// <summary>Gets the hash of the ProTx registration.</summary>
    public string ProTxHash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(proTxHash, nameof(proTxHash));
}
