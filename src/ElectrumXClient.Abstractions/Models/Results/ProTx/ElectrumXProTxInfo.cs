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
