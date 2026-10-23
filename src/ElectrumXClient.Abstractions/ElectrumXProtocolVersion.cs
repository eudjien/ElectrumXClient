using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>Represents an Electrum protocol version with major, minor, and optional patch components.</summary>
public sealed class ElectrumXProtocolVersion : IComparable<ElectrumXProtocolVersion>, IEquatable<ElectrumXProtocolVersion>
{
    /// <summary>Creates a protocol version from major and minor components.</summary>
    public ElectrumXProtocolVersion(int major, int minor)
    {
        ValidateComponent(major, nameof(major));
        ValidateComponent(minor, nameof(minor));

        Major = major;
        Minor = minor;
        Patch = -1;
    }

    /// <summary>Creates a protocol version from major, minor, and patch components.</summary>
    public ElectrumXProtocolVersion(int major, int minor, int patch)
    {
        if (major < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(major));
        }

        if (minor < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minor));
        }

        ValidateComponent(patch, nameof(patch));

        Major = major;
        Minor = minor;
        Patch = patch;
    }

    /// <summary>Gets the major component.</summary>
    public int Major { get; }

    /// <summary>Gets the minor component.</summary>
    public int Minor { get; }

    /// <summary>Gets the optional patch component, or -1 when unspecified.</summary>
    public int Patch { get; }

    /// <inheritdoc />
    public int CompareTo(ElectrumXProtocolVersion? other)
    {
        if (other is null)
        {
            return 1;
        }

        int majorComparison = Major.CompareTo(other.Major);
        if (majorComparison != 0)
        {
            return majorComparison;
        }

        int minorComparison = Minor.CompareTo(other.Minor);
        return minorComparison != 0
            ? minorComparison
            : GetComparablePatch(Patch).CompareTo(GetComparablePatch(other.Patch));
    }

    /// <inheritdoc />
    public bool Equals(ElectrumXProtocolVersion? other)
    {
        return other is not null && CompareTo(other) == 0;
    }

    private static void ValidateComponent(int value, string parameterName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName);
        }
    }

    /// <summary>Parses a protocol version with two or three numeric components.</summary>
    public static ElectrumXProtocolVersion Parse(string input)
    {
        if (!TryParse(input, out var result))
        {
            throw new FormatException("The value must be a protocol version with two or three numeric components.");
        }

        return result;
    }

    /// <summary>Attempts to parse a protocol version with two or three numeric components.</summary>
    public static bool TryParse(string? input, [NotNullWhen(true)] out ElectrumXProtocolVersion? result)
    {
        result = null;

        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        string[] components = input.Split('.');
        if (components.Length is < 2 or > 3 ||
            !int.TryParse(components[0], NumberStyles.None, CultureInfo.InvariantCulture, out int major) ||
            !int.TryParse(components[1], NumberStyles.None, CultureInfo.InvariantCulture, out int minor))
        {
            return false;
        }

        int patch = -1;
        if (components.Length == 3 &&
            !int.TryParse(components[2], NumberStyles.None, CultureInfo.InvariantCulture, out patch))
        {
            return false;
        }

        result = components.Length == 3
            ? new ElectrumXProtocolVersion(major, minor, patch)
            : new ElectrumXProtocolVersion(major, minor);
        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is ElectrumXProtocolVersion other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Major, Minor, GetComparablePatch(Patch));
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Patch < 0
            ? Major.ToString(CultureInfo.InvariantCulture) + "." + Minor.ToString(CultureInfo.InvariantCulture)
            : Major.ToString(CultureInfo.InvariantCulture) + "." + Minor.ToString(CultureInfo.InvariantCulture) +
              "." + Patch.ToString(CultureInfo.InvariantCulture);
    }

    private static int Compare(ElectrumXProtocolVersion? left, ElectrumXProtocolVersion? right)
    {
        if (left is null)
        {
            return right is null ? 0 : -1;
        }

        return left.CompareTo(right);
    }

    private static int GetComparablePatch(int patch)
    {
        return patch < 0 ? 0 : patch;
    }

    /// <summary>Determines whether the left protocol version is less than the right.</summary>
    public static bool operator <(ElectrumXProtocolVersion? left, ElectrumXProtocolVersion? right)
    {
        return Compare(left, right) < 0;
    }

    /// <summary>Determines whether the left protocol version is less than or equal to the right.</summary>
    public static bool operator <=(ElectrumXProtocolVersion? left, ElectrumXProtocolVersion? right)
    {
        return Compare(left, right) <= 0;
    }

    /// <summary>Determines whether the left protocol version is greater than the right.</summary>
    public static bool operator >(ElectrumXProtocolVersion? left, ElectrumXProtocolVersion? right)
    {
        return Compare(left, right) > 0;
    }

    /// <summary>Determines whether the left protocol version is greater than or equal to the right.</summary>
    public static bool operator >=(ElectrumXProtocolVersion? left, ElectrumXProtocolVersion? right)
    {
        return Compare(left, right) >= 0;
    }

    /// <summary>Determines whether two protocol versions are equal.</summary>
    public static bool operator ==(ElectrumXProtocolVersion? left, ElectrumXProtocolVersion? right)
    {
        return Equals(left, right);
    }

    /// <summary>Determines whether two protocol versions are not equal.</summary>
    public static bool operator !=(ElectrumXProtocolVersion? left, ElectrumXProtocolVersion? right)
    {
        return !Equals(left, right);
    }
}
