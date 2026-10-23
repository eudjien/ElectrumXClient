namespace NorthGomel.ElectrumXClient.Abstractions.JsonRpc;

/// <summary>Contains the parsed fields and validation state of a JSON-RPC message envelope.</summary>
public sealed class JsonRpcHeader(
    JsonRpcId? id,
    bool hasId,
    string? jsonRpcVersion,
    bool hasMethod,
    string? method,
    bool hasError,
    bool isErrorObject,
    bool hasResult,
    JsonRpcHeader.JsonRpcParametersKind parametersKind)
{

    /// <summary>Identifies the kind of a JSON-RPC message.</summary>
    public enum JsonRpcMessageType
    {
        Response,
        Notification,
        Request
    }

    /// <summary>Describes the JSON shape of a JSON-RPC params value.</summary>
    public enum JsonRpcParametersKind
    {
        Missing,
        Array,
        Object,
        Other
    }

    public JsonRpcId? Id { get; } = id;

    public bool HasId { get; } = hasId;

    public string? JsonRpcVersion { get; } = jsonRpcVersion;

    public bool HasMethod { get; } = hasMethod;

    public string? Method { get; } = method;

    public bool HasError { get; } = hasError;

    public bool IsErrorObject { get; } = isErrorObject;

    public bool HasResult { get; } = hasResult;

    public JsonRpcParametersKind ParametersKind { get; } = parametersKind;
}
