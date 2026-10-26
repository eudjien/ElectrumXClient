using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The daemon-specific verbose result returned by blockchain.transaction.broadcast_package.</summary>
public sealed class ElectrumXVerboseTransactionPackageBroadcast(
    string? packageMessage,
    IReadOnlyDictionary<string, ElectrumXVerboseTransactionPackageTransactionEntry> transactionResults,
    IReadOnlyList<string>? replacedTransactions,
    IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null) : ElectrumXResult
{
    /// <summary>Overall package submission message returned by bitcoind.</summary>
    public string? PackageMessage { get; } = packageMessage;

    /// <summary>Per-transaction results keyed by witness transaction ID.</summary>
    public IReadOnlyDictionary<string, ElectrumXVerboseTransactionPackageTransactionEntry> TransactionResults { get; } =
        transactionResults;

    /// <summary>Transaction IDs of transactions replaced by the submitted package.</summary>
    public IReadOnlyList<string>? ReplacedTransactions { get; } = replacedTransactions;

    /// <summary>Unrecognized top-level properties returned by the daemon.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; } =
        AdditionalPropertiesHelper.Copy(additionalProperties, IsKnownProperty);

    private static bool IsKnownProperty(string name)
    {
        return name is ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.PackageMessage or
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.TransactionResults or
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.ReplacedTransactions;
    }
}

/// <summary>Verbose package submission details for one transaction.</summary>
public sealed class ElectrumXVerboseTransactionPackageTransactionEntry(
    string? transactionId,
    string? otherWitnessTransactionId,
    long? virtualSize,
    ElectrumXVerboseTransactionPackageFees? fees,
    string? error,
    IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null)
{
    /// <summary>Transaction ID.</summary>
    public string? TransactionId { get; } = transactionId;

    /// <summary>Witness transaction ID of a different transaction with the same transaction ID, when present.</summary>
    public string? OtherWitnessTransactionId { get; } = otherWitnessTransactionId;

    /// <summary>Sigops-adjusted virtual transaction size.</summary>
    public long? VirtualSize { get; } = virtualSize;

    /// <summary>Transaction fee details.</summary>
    public ElectrumXVerboseTransactionPackageFees? Fees { get; } = fees;

    /// <summary>Mempool rejection reason, when the transaction was rejected.</summary>
    public string? Error { get; } = error;

    /// <summary>Unrecognized transaction-result properties returned by the daemon.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; } =
        AdditionalPropertiesHelper.Copy(additionalProperties, IsKnownProperty);

    private static bool IsKnownProperty(string name)
    {
        return name is ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.TransactionId or
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.OtherWitnessTransactionId or
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.VirtualSize or
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.Fees or
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.Error;
    }
}

/// <summary>Fee details for a transaction in a verbose package submission response.</summary>
public sealed class ElectrumXVerboseTransactionPackageFees(
    decimal? baseFee,
    decimal? effectiveFeeRate,
    IReadOnlyList<string>? effectiveIncludes,
    IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties = null)
{
    /// <summary>Transaction fee in BTC.</summary>
    public decimal? BaseFee { get; } = baseFee;

    /// <summary>Effective fee rate in BTC per kvB.</summary>
    public decimal? EffectiveFeeRate { get; } = effectiveFeeRate;

    /// <summary>Witness transaction IDs included in the effective fee-rate calculation.</summary>
    public IReadOnlyList<string>? EffectiveIncludes { get; } = effectiveIncludes;

    /// <summary>Unrecognized fee properties returned by the daemon.</summary>
    public IReadOnlyDictionary<string, ElectrumXJsonValue> AdditionalProperties { get; } =
        AdditionalPropertiesHelper.Copy(additionalProperties, IsKnownProperty);

    private static bool IsKnownProperty(string name)
    {
        return name is ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.BaseFee or
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.EffectiveFeeRate or
            ElectrumXJsonPropertyNames.VerboseTransactionPackageBroadcastResult.EffectiveIncludes;
    }
}
