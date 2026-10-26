namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.ProTx;

/// <summary>Detailed deterministic masternode information returned by protx.info.</summary>
public sealed class ElectrumXProTxInfo : ElectrumXResult
{
    /// <summary>Creates detailed deterministic masternode information returned by <c>protx.info</c>.</summary>
    public ElectrumXProTxInfo(string? proTxHash = null, string? collateralHash = null, long? collateralIndex = null, decimal? operatorReward = null,
        ElectrumXProTxState? state = null, long? confirmations = null)
    {
        ProTxHash = proTxHash;
        CollateralHash = collateralHash;
        CollateralIndex = collateralIndex;
        OperatorReward = operatorReward;
        State = state;
        Confirmations = confirmations;
    }

    /// <summary>32-byte hash of the initial ProRegTx registration, encoded as 64 hexadecimal characters.</summary>
    public string? ProTxHash { get; }

    /// <summary>32-byte collateral transaction hash encoded as 64 hexadecimal characters.</summary>
    public string? CollateralHash { get; }

    /// <summary>Output index of the collateral transaction.</summary>
    public long? CollateralIndex { get; }

    /// <summary>Operator reward fraction or value returned by the Dash-compatible server.</summary>
    public decimal? OperatorReward { get; }

    /// <summary>Current deterministic masternode state.</summary>
    public ElectrumXProTxState? State { get; }

    /// <summary>Number of confirmations for the registration transaction.</summary>
    public long? Confirmations { get; }
}

/// <summary>Mutable state and keys for a deterministic masterNode.</summary>
public sealed class ElectrumXProTxState : ElectrumXResult
{
    /// <summary>Creates the mutable state and keys for a deterministic masternode.</summary>
    public ElectrumXProTxState(long? registeredHeight = null, long? lastPaidHeight = null, long? poSePenalty = null, long? poSeRevivedHeight = null,
        long? poSeBanHeight = null, long? revocationReason = null, string? ownerKeyId = null, string? operatorPublicKey = null,
        string? votingKeyId = null, string? ownerKeyAddress = null, string? votingKeyAddress = null, string? address = null,
        string? payoutAddress = null)
    {
        RegisteredHeight = registeredHeight;
        LastPaidHeight = lastPaidHeight;
        PoSePenalty = poSePenalty;
        PoSeRevivedHeight = poSeRevivedHeight;
        PoSeBanHeight = poSeBanHeight;
        RevocationReason = revocationReason;
        OwnerKeyId = ownerKeyId;
        OperatorPublicKey = operatorPublicKey;
        VotingKeyId = votingKeyId;
        OwnerKeyAddress = ownerKeyAddress;
        VotingKeyAddress = votingKeyAddress;
        Address = address;
        PayoutAddress = payoutAddress;
    }

    /// <summary>Block height at which the masternode was registered.</summary>
    public long? RegisteredHeight { get; }

    /// <summary>Block height at which the masternode was last paid.</summary>
    public long? LastPaidHeight { get; }

    /// <summary>Current Proof of Service penalty.</summary>
    public long? PoSePenalty { get; }

    /// <summary>Block height at which the masternode was last revived from a PoSe ban.</summary>
    public long? PoSeRevivedHeight { get; }

    /// <summary>Block height at which the masternode was banned by PoSe.</summary>
    public long? PoSeBanHeight { get; }

    /// <summary>Reason code for the latest revocation.</summary>
    public long? RevocationReason { get; }

    /// <summary>20-byte owner key identifier encoded as 40 hexadecimal characters.</summary>
    public string? OwnerKeyId { get; }

    /// <summary>48-byte BLS public key encoded as 96 hexadecimal characters.</summary>
    public string? OperatorPublicKey { get; }

    /// <summary>20-byte voting key identifier encoded as 40 hexadecimal characters.</summary>
    public string? VotingKeyId { get; }

    /// <summary>Dash address string for the owner's payout key (Base58 on mainnet).</summary>
    public string? OwnerKeyAddress { get; }

    /// <summary>Dash address string for the voting key (Base58 on mainnet).</summary>
    public string? VotingKeyAddress { get; }

    /// <summary>Masternode IP address and port in <c>host:port</c> notation.</summary>
    public string? Address { get; }

    /// <summary>Dash address receiving the masternode's block reward payout (Base58 on mainnet).</summary>
    public string? PayoutAddress { get; }
}
