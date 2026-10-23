namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>CashToken non-fungible token metadata.</summary>
public sealed class ElectrumXTokenNft(ElectrumXTokenNftCapability capability, string? commitment) : ElectrumXResult
{
    /// <summary>NFT capability: <c>none</c>, <c>mutable</c>, or <c>minting</c>.</summary>
    public ElectrumXTokenNftCapability Capability { get; } = capability;

    /// <summary>Optional NFT commitment bytes encoded as hexadecimal; its length is token-defined.</summary>
    public string? Commitment { get; } = commitment;
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
