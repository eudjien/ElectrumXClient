using System;

namespace NorthGomel.ElectrumXClient.Abstractions.Exceptions;

/// <summary>Represents an ElectrumX-specific failure that is surfaced to callers.</summary>
public abstract class ElectrumXException : Exception
{
    protected ElectrumXException(string message)
        : base(message)
    {
    }

    protected ElectrumXException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected ElectrumXException(string format, params object[] args)
        : base(string.Format(format, args))
    {
    }

    protected ElectrumXException(string format, Exception innerException, params object[] args)
        : base(string.Format(format, args), innerException)
    {
    }
}
