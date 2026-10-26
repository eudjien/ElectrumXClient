namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>The current status of a subscribed script hash.</summary>
public sealed class ElectrumXScriptHashSubscription : ElectrumXResult
{
    /// <summary>Creates a result containing the current script-hash status, if any.</summary>
    public ElectrumXScriptHashSubscription(string? value)
    {
        Value = value;
    }

    /// <summary>
    ///     64-character hexadecimal history-status digest (SHA-256 when advertised by the server), or <see langword="null" /> when the script hash has no
    ///     status.
    /// </summary>
    public string? Value { get; }

    /// <summary>Converts the response to its script-hash status.</summary>
    /// <param name="response">The subscription response.</param>
    /// <returns>The current status, or <see langword="null" /> when unused.</returns>
    public static implicit operator string?(ElectrumXScriptHashSubscription response)
    {
        return response.Value;
    }
}
