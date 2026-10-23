namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The nullable address returned by blockchain.utxo.get_address.</summary>
public sealed class ElectrumXUtxoAddress(string? address) : ElectrumXResult
{
    /// <summary>Chain-specific address string (for example, CashAddr or Base58), or <see langword="null" /> when the UTXO has no address.</summary>
    public string? Address { get; } = address;
}
