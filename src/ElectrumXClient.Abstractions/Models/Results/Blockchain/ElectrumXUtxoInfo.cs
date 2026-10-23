namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Information about a specific unspent transaction output.</summary>
public sealed class ElectrumXUtxoInfo(
    long? confirmedHeight,
    string scriptHash,
    long value,
    ElectrumXTokenData? tokenData) : ElectrumXResult
{
    /// <summary>Confirmation height, or <see langword="null" /> when the output is unconfirmed.</summary>
    public long? ConfirmedHeight { get; } = confirmedHeight;

    /// <summary>Script hash: 32-byte digest of the locking script (SHA-256 when advertised by the server), reversed and encoded as 64 hexadecimal characters.</summary>
    public string ScriptHash { get; } = scriptHash;

    /// <summary>Output value in minimum coin units.</summary>
    public long Value { get; } = value;

    /// <summary>Optional CashToken metadata.</summary>
    public ElectrumXTokenData? TokenData { get; } = tokenData;
}
