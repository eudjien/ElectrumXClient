namespace NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;

/// <summary>CashToken filter applied to balance and unspent-output queries.</summary>
public enum ElectrumXTokenFilter
{
    /// <summary>Include outputs with and without CashTokens.</summary>
    IncludeTokens,

    /// <summary>Exclude outputs containing CashTokens.</summary>
    ExcludeTokens,

    /// <summary>Include only outputs containing CashTokens.</summary>
    TokensOnly
}
