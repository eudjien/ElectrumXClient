using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

// TODO: transactionOrId
public sealed class TscMerkleWithTransactionHexResultJsonConverter
    : TscMerkleResultJsonConverter<ElectrumXTscMerkleWithTransactionHex>
{
    public override bool HandleNull => true;

    protected override ElectrumXTscMerkleWithTransactionHex Create(
        bool composite,
        int index,
        IReadOnlyList<string> nodes,
        string proofType,
        string target,
        TscMerkleTargetType targetType,
        string transactionOrId)
    {
        return new ElectrumXTscMerkleWithTransactionHex(
            composite, index, nodes, proofType, target, targetType, transactionOrId);
    }

    protected override string GetTransactionOrId(ElectrumXTscMerkleWithTransactionHex response)
    {
        return response.TransactionHex;
    }
}
