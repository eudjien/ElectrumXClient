using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Exceptions;

/// <summary>
///     Represents a failure of the underlying network transport while connecting to or communicating with an ElectrumX server.
/// </summary>
/// <remarks>
///     This exception is raised when a transport error prevents establishing a connection, interrupts protocol negotiation,
///     or occurs while sending or receiving a request. Request and connection timeouts use
///     <see cref="ElectrumXTimeoutException" /> instead; malformed protocol data and server-reported RPC errors use their
///     corresponding exception types.
/// </remarks>
public sealed class ElectrumXConnectionException : ElectrumXException
{
    /// <summary>Creates a transport exception with a descriptive message.</summary>
    public ElectrumXConnectionException(string message) : base(message)
    {
    }

    /// <summary>Creates a transport exception with a descriptive message and the underlying transport error.</summary>
    public ElectrumXConnectionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>Creates a connection exception using a formatted message.</summary>
    public ElectrumXConnectionException(string format, params object[] args) : base(format, args)
    {
    }

    /// <summary>Creates a connection exception using a formatted message and inner exception.</summary>
    public ElectrumXConnectionException(string format, Exception innerException, params object[] args)
        : base(format, innerException, args)
    {
    }
}
