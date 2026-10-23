namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;

/// <summary>A deterministic masterNode list entry included in a ProTx diff.</summary>
public sealed class ElectrumXProTxMasterNode : ElectrumXResult
{
    /// <summary>Creates a deterministic masternode list entry returned by <c>protx.diff</c>.</summary>
    public ElectrumXProTxMasterNode(string? proRegTxHash = null, string? confirmedHash = null, string? service = null, string? operatorPublicKey = null,
        string? votingKeyId = null, bool? isValid = null)
    {
        ProRegTxHash = proRegTxHash;
        ConfirmedHash = confirmedHash;
        Service = service;
        OperatorPublicKey = operatorPublicKey;
        VotingKeyId = votingKeyId;
        IsValid = isValid;
    }

    /// <summary>32-byte ProRegTx hash encoded as 64 hexadecimal characters.</summary>
    public string? ProRegTxHash { get; }

    /// <summary>32-byte block hash encoded as 64 hexadecimal characters.</summary>
    public string? ConfirmedHash { get; }

    /// <summary>Masternode IP address and port in <c>host:port</c> notation.</summary>
    public string? Service { get; }

    /// <summary>48-byte BLS public key encoded as 96 hexadecimal characters.</summary>
    public string? OperatorPublicKey { get; }

    /// <summary>20-byte voting key identifier encoded as 40 hexadecimal characters.</summary>
    public string? VotingKeyId { get; }

    /// <summary>Whether the masternode is valid in the returned list.</summary>
    public bool? IsValid { get; }
}
