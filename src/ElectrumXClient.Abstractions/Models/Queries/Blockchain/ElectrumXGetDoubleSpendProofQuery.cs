namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>Query parameters for retrieving a double-spend proof.</summary>
public sealed class ElectrumXGetDoubleSpendProofQuery(string hash)
    : ElectrumXQuery
{
    /// <summary>Gets the double-spend proof hash.</summary>
    public string Hash { get; } = ArgumentGuard.ValidateAndNormalizeSha256Hash(hash, nameof(hash));
}
