namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>CashToken metadata associated with an unspent transaction output.</summary>
public sealed class ElectrumXTokenData(long amount, string category, ElectrumXTokenNft? nft = null) : ElectrumXResult
{

    /// <summary>Fungible token quantity represented as a non-negative integer.</summary>
    public long Amount { get; } = amount;

    /// <summary>32-byte CashToken category identifier encoded as 64 hexadecimal characters.</summary>
    public string Category { get; } = category;

    /// <summary>Optional non-fungible token metadata.</summary>
    public ElectrumXTokenNft? Nft { get; } = nft;
}

/// <summary>CashToken non-fungible token metadata.</summary>
public sealed class ElectrumXTokenNft(ElectrumXTokenNftCapability capability, string commitment) : ElectrumXResult
{
    /// <summary>NFT capability: <c>none</c>, <c>mutable</c>, or <c>minting</c>.</summary>
    public ElectrumXTokenNftCapability Capability { get; } = capability;

    /// <summary>NFT commitment bytes encoded as hexadecimal; empty when the NFT has no commitment bytes.</summary>
    public string Commitment { get; } = commitment;
}

/// <summary>CashToken non-fungible token capability.</summary>
public enum ElectrumXTokenNftCapability
{
    /// <summary>The NFT has no mutable or minting capability.</summary>
    None,

    /// <summary>The NFT can be modified.</summary>
    Mutable,

    /// <summary>The NFT can create additional NFTs in the token category.</summary>
    Minting
}
