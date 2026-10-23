using System;
using System.Diagnostics.CodeAnalysis;

namespace NorthGomel.ElectrumXClient.Abstractions;

internal static class ArgumentGuard
{
    internal static void ThrowIfNull([NotNull] object? argument, string parameterName)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }

    internal static void ThrowIfNullOrWhiteSpace([NotNull] string? argument, string parameterName)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(parameterName);
        }

        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", parameterName);
        }
    }

    internal static void ThrowIfNegative(int argument, string parameterName)
    {
        if (argument < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, argument, "Value must be non-negative.");
        }
    }

    internal static void ThrowIfNegative(long argument, string parameterName)
    {
        if (argument < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, argument, "Value must be non-negative.");
        }
    }

    internal static string ValidateAndNormalizeHex(string? argument, string parameterName)
    {
        ThrowIfNullOrWhiteSpace(argument, parameterName);

        string normalizedArgument = argument.Trim();
        if (normalizedArgument.Length % 2 != 0)
        {
            throw new ArgumentException("Value must be even-length hexadecimal.", parameterName);
        }

        foreach (char character in normalizedArgument)
        {
            if (!Uri.IsHexDigit(character))
            {
                throw new ArgumentException("Value must contain only hexadecimal characters.", parameterName);
            }
        }

        return normalizedArgument.ToLowerInvariant();
    }

    internal static string ValidateAndNormalizeSha256Hash(string? argument, string parameterName)
    {
        string normalizedArgument = ValidateAndNormalizeHex(argument, parameterName);
        if (normalizedArgument.Length != 64)
        {
            throw new ArgumentException("A SHA-256 hash must contain exactly 64 hexadecimal characters.", parameterName);
        }

        return normalizedArgument;
    }
}
