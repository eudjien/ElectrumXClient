using System;

namespace NorthGomel.ElectrumXClient.Abstractions.JsonRpc;

/// <summary>
///     Defines the concrete JSON-RPC identifier kind.
/// </summary>
public enum JsonRpcIdKind
{
    Null,
    Number,
    String
}

/// <summary>
///     JSON-RPC identifier that may be a number, a string, or null.
/// </summary>
public sealed class JsonRpcId : IEquatable<JsonRpcId>
{
    private readonly JsonRpcIdKind _kind;
    private readonly object _value;

    private JsonRpcId()
    {
        _value = new object();
        _kind = JsonRpcIdKind.Null;
    }

    private JsonRpcId(long number)
    {
        _value = number;
        _kind = JsonRpcIdKind.Number;
    }

    private JsonRpcId(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _kind = JsonRpcIdKind.String;
    }

    /// <summary>
    ///     A null JSON-RPC identifier value.
    /// </summary>
    public object? Null => null;

    /// <summary>
    ///     Numeric JSON-RPC id when the identifier is represented as a number.
    /// </summary>
    public long? Number => _kind == JsonRpcIdKind.Number ? (long?)_value : null;

    /// <summary>
    ///     String JSON-RPC id when the identifier is represented as a string.
    /// </summary>
    public string? String => _kind == JsonRpcIdKind.String ? (string?)_value : null;

    /// <summary>
    ///     Describes which JSON-RPC identifier variant is currently stored.
    /// </summary>
    public JsonRpcIdKind Kind => _kind;

    public bool Equals(JsonRpcId? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return Equals(_value, other._value);
    }

    public static JsonRpcId FromNumber(long number)
    {
        return new JsonRpcId(number);
    }

    public static JsonRpcId FromNull()
    {
        return new JsonRpcId();
    }

    public static JsonRpcId FromString(string value)
    {
        return new JsonRpcId(value);
    }

    public static JsonRpcId FromValue(object? value)
    {
        return value switch
        {
            null => new JsonRpcId(),
            JsonRpcId jsonRpcId => jsonRpcId,
            long number => new JsonRpcId(number),
            int number => new JsonRpcId(number),
            short number => new JsonRpcId(number),
            byte number => new JsonRpcId(number),
            sbyte number => new JsonRpcId(number),
            ushort number => new JsonRpcId(number),
            uint number => new JsonRpcId(number),
            ulong number and <= long.MaxValue => new JsonRpcId((long)number),
            string text => new JsonRpcId(text),
            _ => throw new ArgumentException("The value is not a valid JSON-RPC identifier.", nameof(value))
        };
    }

    public static bool TryFromValue(object? value, out JsonRpcId? id)
    {
        try
        {
            id = FromValue(value);
            return true;
        }
        catch
        {
            id = null;
            return false;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is JsonRpcId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value?.GetHashCode() ?? 0;
    }

    public override string ToString()
    {
        return _value switch
        {
            null => string.Empty,
            long number => number.ToString(),
            string text => text,
            _ => throw new InvalidOperationException("The JSON-RPC identifier value is invalid.")
        };
    }
}
