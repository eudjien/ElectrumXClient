using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>TSC Merkle proof containing the raw transaction hex.</summary>
public sealed class ElectrumXTscMerkleWithTransactionHex : ElectrumXTscMerkle
{
    /// <summary>Creates a TSC Merkle proof that identifies its transaction by serialized data.</summary>
    public ElectrumXTscMerkleWithTransactionHex(bool composite, int index, IReadOnlyList<string> nodes, string proofType, string target,
        TscMerkleTargetType targetType, string transactionHex)
        : base(composite, index, nodes, proofType, target, targetType)
    {
        TransactionHex = transactionHex;
    }

    /// <summary>Serialized transaction bytes encoded as hexadecimal.</summary>
    public string TransactionHex { get; }
}
