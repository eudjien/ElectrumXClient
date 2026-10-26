namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;

/// <summary>Indicates whether a script-hash subscription was removed.</summary>
public sealed class ElectrumXScriptHashUnsubscription : ElectrumXResult
{
    /// <summary>Creates a result indicating whether a script-hash subscription was removed.</summary>
    public ElectrumXScriptHashUnsubscription(bool value)
    {
        Value = value;
    }

    /// <summary><see langword="true" /> if the subscription was removed; otherwise, <see langword="false" />.</summary>
    public bool Value { get; }

    /// <summary>Converts the response to the server's unsubscription result.</summary>
    /// <param name="response">The unsubscription response.</param>
    /// <returns><see langword="true" /> if the subscription was removed.</returns>
    public static implicit operator bool(ElectrumXScriptHashUnsubscription response)
    {
        return response.Value;
    }
}
