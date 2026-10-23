using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class TscMerkleWithTransactionHashResultJsonConverter
    : TscMerkleResultJsonConverter<ElectrumXTscMerkleWithTransactionHash>
{
    public override bool HandleNull => true;

    protected override ElectrumXTscMerkleWithTransactionHash Create(
        bool composite,
        int index,
        IReadOnlyList<string> nodes,
        string proofType,
        string target,
        TscMerkleTargetType targetType,
        string transactionOrId)
    {
        return new ElectrumXTscMerkleWithTransactionHash(
            composite, index, nodes, proofType, target, targetType, transactionOrId);
    }

    protected override string GetTransactionOrId(ElectrumXTscMerkleWithTransactionHash response)
    {
        return response.TransactionHash;
    }
}
