namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results;

/// <summary>A result containing a protocol value whose shape varies by server implementation.</summary>
public sealed class ElectrumXJsonValueResult(ElectrumXJsonValue value) : ElectrumXResult
{
    /// <summary>The JSON result, including <see cref="ElectrumXJsonValue.NullValue" /> for a JSON null.</summary>
    public ElectrumXJsonValue Value { get; } = value;
}
