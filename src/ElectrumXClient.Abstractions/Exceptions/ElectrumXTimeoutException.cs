using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Exceptions;

/// <summary>Represents an operation that exceeded its configured ElectrumX timeout.</summary>
/// <remarks>
///     This exception is thrown when establishing or negotiating a connection exceeds the configured connection timeout,
///     or when an RPC request exceeds the configured request timeout. Caller-requested cancellation is reported as an
///     <see cref="OperationCanceledException" /> instead; transport failures that are not timeouts use
///     <see cref="ElectrumXConnectionException" />.
/// </remarks>
public sealed class ElectrumXTimeoutException : ElectrumXException
{
    /// <summary>Creates a timeout exception with a descriptive message.</summary>
    public ElectrumXTimeoutException(string message)
        : base(message)
    {
    }

    /// <summary>Creates a timeout exception with a descriptive message and the underlying timeout error.</summary>
    public ElectrumXTimeoutException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>Creates a timeout exception using a formatted message.</summary>
    public ElectrumXTimeoutException(string format, params object[] args)
        : base(format, args)
    {
    }

    /// <summary>Creates a timeout exception using a formatted message and inner exception.</summary>
    public ElectrumXTimeoutException(string format, Exception innerException, params object[] args)
        : base(format, innerException, args)
    {
    }
}
