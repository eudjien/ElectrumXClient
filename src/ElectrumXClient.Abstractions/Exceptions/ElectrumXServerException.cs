using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Exceptions;

/// <summary>Represents an RPC error returned by the ElectrumX server in a JSON-RPC response.</summary>
/// <remarks>
///     This exception is raised after a response has been successfully decoded. Its message combines the server's
///     error message and code when an error payload is present, or indicates that the payload is missing. <see cref="Response" />
///     and <see cref="Error" /> retain access to the complete response and error details. Transport failures, timeouts,
///     and malformed or unsupported protocol messages use their corresponding exception types instead.
/// </remarks>
public sealed class ElectrumXServerException : ElectrumXException
{
    /// <summary>Creates an exception from a JSON-RPC response that represents a server error.</summary>
    /// <param name="response">The failed response returned by the ElectrumX server.</param>
    /// <exception cref="ArgumentNullException"><paramref name="response" /> is <see langword="null" />.</exception>
    public ElectrumXServerException(JsonRpcResponse<ElectrumXResult> response)
        : base(response is null
            ? throw new ArgumentNullException(nameof(response))
            : response.Error is null
                ? "The JSON-RPC error response did not contain an error payload."
                : $"The ElectrumX server returned a JSON-RPC error: '{response.Error.Message}' (code: {response.Error.Code}).")
    {
        Response = response;
    }

    /// <summary>The complete error response returned by the server.</summary>
    public JsonRpcResponse<ElectrumXResult> Response { get; }

    /// <summary>The server-provided JSON-RPC error object.</summary>
    public JsonRpcError Error => Response.Error
                                 ?? throw new InvalidOperationException("The JSON-RPC error response did not contain an error payload.");

    /// <summary>The server-provided JSON-RPC error code.</summary>
    public int? Code => Error.Code;
}
