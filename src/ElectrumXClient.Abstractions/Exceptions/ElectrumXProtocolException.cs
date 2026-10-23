using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Exceptions;

/// <summary>
///     Represents an error while encoding or processing an ElectrumX JSON-RPC protocol message.
/// </summary>
/// <remarks>
///     This exception is raised when a protocol message cannot be serialized or deserialized, or when a response or
///     notification is malformed, incomplete, or uses unsupported protocol values. It can also indicate an invalid
///     WebSocket message type. Transport failures, timeouts, and server-reported JSON-RPC errors use their corresponding
///     exception types instead.
/// </remarks>
public sealed class ElectrumXProtocolException : ElectrumXException
{
    /// <summary>Creates a protocol exception with a descriptive message.</summary>
    public ElectrumXProtocolException(string message)
        : base(message)
    {
    }

    /// <summary>Creates a protocol exception with a descriptive message and the underlying error.</summary>
    public ElectrumXProtocolException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>Creates a protocol exception using a formatted message.</summary>
    public ElectrumXProtocolException(string format, params object[] args)
        : base(format, args)
    {
    }

    /// <summary>Creates a protocol exception using a formatted message and inner exception.</summary>
    public ElectrumXProtocolException(string format, Exception innerException, params object[] args)
        : base(format, innerException, args)
    {
    }
}
