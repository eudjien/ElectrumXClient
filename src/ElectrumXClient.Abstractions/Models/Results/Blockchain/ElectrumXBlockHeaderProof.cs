using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Merkle proof that a block header is included up to a checkpoint root.</summary>
public sealed class ElectrumXBlockHeaderProof : ElectrumXResult
{
    /// <summary>Creates a block-header inclusion proof up to a checkpoint.</summary>
    public ElectrumXBlockHeaderProof(IReadOnlyList<string> branch, string header, string root)
    {
        Branch = branch;
        Header = header;
        Root = root;
    }

    /// <summary>32-byte header hashes encoded as 64 hexadecimal characters, deepest pairing first.</summary>
    public IReadOnlyList<string> Branch { get; }

    /// <summary>Raw 80-byte block header encoded as 160 hexadecimal characters.</summary>
    public string Header { get; }

    /// <summary>32-byte Merkle root of headers encoded as 64 hexadecimal characters.</summary>
    public string Root { get; }
}
