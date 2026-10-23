namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>CashToken metadata associated with an unspent transaction output.</summary>
public sealed class ElectrumXTokenData(string amount, string category, ElectrumXTokenNft? nft = null) : ElectrumXResult
{
    /// <summary>Fungible token quantity represented as a decimal string.</summary>
    public string Amount { get; } = amount;

    /// <summary>32-byte CashToken category identifier encoded as 64 hexadecimal characters.</summary>
    public string Category { get; } = category;

    /// <summary>Optional non-fungible token metadata.</summary>
    public ElectrumXTokenNft? Nft { get; } = nft;
}
