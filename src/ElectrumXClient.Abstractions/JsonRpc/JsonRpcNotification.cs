namespace NorthGomel.ElectrumXClient.Abstractions.JsonRpc;

/// <summary>Represents a JSON-RPC 2.0 notification envelope.</summary>
public sealed class JsonRpcNotification<TParams>(string jsonRpcVersion, string method, TParams? parameters)
    where TParams : class
{
    public string JsonRpcVersion { get; } = jsonRpcVersion;

    public string Method { get; } = method;

    public TParams? Parameters { get; } = parameters;
}
