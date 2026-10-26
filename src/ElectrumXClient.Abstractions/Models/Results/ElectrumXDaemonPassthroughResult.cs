namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results;

/// <summary>The raw JSON result returned by daemon.passthrough.</summary>
public sealed class ElectrumXDaemonPassthroughResult(ElectrumXJsonValue value) : ElectrumXResult
{
    /// <summary>The daemon's JSON result, whose shape depends on the forwarded method.</summary>
    public ElectrumXJsonValue Value { get; } = value;

    /// <summary>Converts the response to its daemon-provided JSON value.</summary>
    /// <param name="response">The daemon passthrough response.</param>
    /// <returns>The JSON value returned by the daemon.</returns>
    public static implicit operator ElectrumXJsonValue(ElectrumXDaemonPassthroughResult response)
    {
        return response.Value;
    }
}
