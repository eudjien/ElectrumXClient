using System.Collections.Generic;

namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>A consecutive batch of block headers, optionally accompanied by a checkpoint proof.</summary>
public sealed class ElectrumXBlockHeaders : ElectrumXResult
{
    /// <summary>Creates a result containing a consecutive block-header range and optional checkpoint proof.</summary>
    public ElectrumXBlockHeaders(
        int count,
        string hex,
        int maximum,
        string? root = null,
        IReadOnlyList<string>? branch = null,
        IReadOnlyList<string>? headers = null)
    {
        Count = count;
        Hex = hex;
        Maximum = maximum;
        Root = root;
        Branch = branch;
        Headers = headers;
    }

    /// <summary>Number of block headers returned, which may be less than requested.</summary>
    public int Count { get; }

    /// <summary>Returned raw 80-byte block headers concatenated in order and encoded as hexadecimal.</summary>
    public string Hex { get; }

    /// <summary>Raw 80-byte block headers returned as individual hexadecimal strings by protocol version 1.6 or later.</summary>
    public IReadOnlyList<string>? Headers { get; }

    /// <summary>Maximum number of headers the server returns in one request.</summary>
    public int Maximum { get; }

    /// <summary>64-character hexadecimal checkpoint Merkle root, or <see langword="null" /> when no proof was requested.</summary>
    public string? Root { get; }

    /// <summary>64-character hexadecimal header hashes in the Merkle branch, or <see langword="null" /> when no proof was requested.</summary>
    public IReadOnlyList<string>? Branch { get; }
}
