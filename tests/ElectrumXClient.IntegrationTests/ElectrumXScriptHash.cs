using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;

namespace NorthGomel.ElectrumXClient.IntegrationTests;

/// <summary>Converts script public keys to ElectrumX script hashes.</summary>
public static class ElectrumXScriptHash
{
    private const string Base58Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
    private const string Bech32Alphabet = "qpzry9x8gf2tvdw0s3jn54khce6mua7l";

    /// <summary>Computes SHA-256 of a script and reverses the digest for ElectrumX representation.</summary>
    public static string FromScriptPubKey(ReadOnlySpan<byte> scriptPubKey)
    {
        Span<byte> hash = stackalloc byte[32];

        if (!SHA256.TryHashData(scriptPubKey, hash, out int bytesWritten) || bytesWritten != hash.Length)
        {
            throw new CryptographicException("Unable to compute the script SHA-256 hash.");
        }

        const string hexadecimalDigits = "0123456789abcdef";
        char[] hexadecimalHash = new char[hash.Length * 2];
        for (int i = 0; i < hash.Length; i++)
        {
            byte value = hash[hash.Length - 1 - i];
            hexadecimalHash[i * 2] = hexadecimalDigits[value >> 4];
            hexadecimalHash[i * 2 + 1] = hexadecimalDigits[value & 0x0f];
        }

        return new string(hexadecimalHash);

    }

    /// <summary>Converts a Bitcoin mainnet, testnet, or regtest address to its ElectrumX script hash.</summary>
    /// <param name="address">A Base58Check P2PKH/P2SH or Bech32/Bech32m SegWit address.</param>
    /// <returns>The lowercase ElectrumX script hash.</returns>
    /// <exception cref="ArgumentException">The address is invalid or uses an unsupported format.</exception>
    public static string FromBitcoinAddress(string address)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(address);

        bool isSegwitAddress =
            address.StartsWith("bc1", StringComparison.OrdinalIgnoreCase) ||
            address.StartsWith("tb1", StringComparison.OrdinalIgnoreCase) ||
            address.StartsWith("bcrt1", StringComparison.OrdinalIgnoreCase);
        byte[] scriptPubKey = isSegwitAddress
            ? DecodeSegwitAddress(address)
            : DecodeBase58Address(address);

        return FromScriptPubKey(scriptPubKey);
    }

    private static byte[] DecodeBase58Address(string address)
    {
        var value = BigInteger.Zero;

        foreach (char character in address)
        {
            int digit = Base58Alphabet.IndexOf(character);
            if (digit < 0)
            {
                throw new ArgumentException("The Bitcoin address contains an invalid Base58 character.", nameof(address));
            }

            value = value * 58 + digit;
        }

        byte[] decodedValue = value.IsZero
            ? []
            : value.ToByteArray(true, true);
        int leadingZeroCount = 0;
        while (leadingZeroCount < address.Length && address[leadingZeroCount] == Base58Alphabet[0])
        {
            leadingZeroCount++;
        }

        byte[] decoded = new byte[leadingZeroCount + decodedValue.Length];
        decodedValue.CopyTo(decoded, leadingZeroCount);

        if (decoded.Length < 5)
        {
            throw new ArgumentException("The Base58Check value is too short.", nameof(address));
        }

        byte[] payload = [.. decoded.AsSpan(0, decoded.Length - 4)];
        byte[] firstHash = SHA256.HashData(payload);
        byte[] checksum = SHA256.HashData(firstHash);

        if (!CryptographicOperations.FixedTimeEquals(decoded.AsSpan(decoded.Length - 4), checksum.AsSpan(0, 4)))
        {
            throw new ArgumentException("The Base58Check address checksum is invalid.", nameof(address));
        }

        if (decoded.Length is 37 or 38 && decoded[0] is 0x80 or 0xef)
        {
            throw new ArgumentException(
                "WIF encodes a private key, not a Bitcoin address; derive an address from its public key first.",
                nameof(address));
        }

        if (decoded.Length != 25)
        {
            throw new ArgumentException(
                "The Base58Check address must decode to a version, 20-byte hash, and checksum.",
                nameof(address));
        }

        Span<byte> scriptPubKey = stackalloc byte[25];
        switch (decoded[0])
        {
            case 0x00:
            case 0x6f:
                scriptPubKey[0] = 0x76;
                scriptPubKey[1] = 0xa9;
                scriptPubKey[2] = 0x14;
                decoded.AsSpan(1, 20).CopyTo(scriptPubKey[3..]);
                scriptPubKey[23] = 0x88;
                scriptPubKey[24] = 0xac;

                return [.. scriptPubKey];

            case 0x05:
            case 0xc4:
                scriptPubKey[0] = 0xa9;
                scriptPubKey[1] = 0x14;
                decoded.AsSpan(1, 20).CopyTo(scriptPubKey[2..]);
                scriptPubKey[22] = 0x87;

                return [.. scriptPubKey[..23]];

            case 0x80:
            case 0xef:
                throw new ArgumentException(
                    "WIF encodes a private key, not a Bitcoin address; derive an address from its public key first.",
                    nameof(address));

            default:
                throw new ArgumentException("The Base58Check version is not a supported Bitcoin address prefix.", nameof(address));
        }
    }

    private static byte[] DecodeSegwitAddress(string address)
    {
        if (address.Length is < 8 or > 90)
        {
            throw new ArgumentException("A SegWit address must contain between 8 and 90 characters.", nameof(address));
        }

        bool hasLowerCase = false;
        bool hasUpperCase = false;
        foreach (char character in address)
        {
            hasLowerCase |= character is >= 'a' and <= 'z';
            hasUpperCase |= character is >= 'A' and <= 'Z';
        }

        if (hasLowerCase && hasUpperCase)
        {
            throw new ArgumentException("A Bech32 address cannot mix uppercase and lowercase characters.", nameof(address));
        }

        string normalizedAddress = address.ToLowerInvariant();
        int separatorIndex = normalizedAddress.LastIndexOf('1');
        if (separatorIndex is < 1 || separatorIndex + 7 > normalizedAddress.Length)
        {
            throw new ArgumentException("The Bech32 address has an invalid separator or checksum.", nameof(address));
        }

        string humanReadablePart = normalizedAddress[..separatorIndex];
        if (humanReadablePart is not ("bc" or "tb" or "bcrt"))
        {
            throw new ArgumentException("The Bech32 address has an unsupported Bitcoin network prefix.", nameof(address));
        }

        var data = new List<byte>(normalizedAddress.Length - separatorIndex - 1);
        foreach (char character in normalizedAddress[(separatorIndex + 1)..])
        {
            int value = Bech32Alphabet.IndexOf(character);
            if (value < 0)
            {
                throw new ArgumentException("The Bech32 address contains an invalid character.", nameof(address));
            }

            data.Add((byte)value);
        }

        if (data.Count < 7)
        {
            throw new ArgumentException("The Bech32 address is missing its witness program or checksum.", nameof(address));
        }

        int witnessVersion = data[0];
        if (witnessVersion > 16)
        {
            throw new ArgumentException("The SegWit witness version must be between 0 and 16.", nameof(address));
        }

        uint checksum = Bech32Polymod(ExpandHumanReadablePart(humanReadablePart), data);
        uint expectedChecksum = witnessVersion == 0 ? 1U : 0x2bc830a3U;
        if (checksum != expectedChecksum)
        {
            throw new ArgumentException("The Bech32/Bech32m address checksum is invalid.", nameof(address));
        }

        byte[] dataBytes = [.. data];
        byte[] witnessProgram = ConvertBits(dataBytes.AsSpan(1, data.Count - 7), 5, 8, false);
        if (witnessProgram.Length is < 2 or > 40 ||
            witnessVersion == 0 && witnessProgram.Length is not (20 or 32))
        {
            throw new ArgumentException("The SegWit witness program has an invalid length.", nameof(address));
        }

        byte[] scriptPubKey = new byte[witnessProgram.Length + 2];
        scriptPubKey[0] = witnessVersion == 0 ? (byte)0 : (byte)(0x50 + witnessVersion);
        scriptPubKey[1] = (byte)witnessProgram.Length;
        witnessProgram.CopyTo(scriptPubKey, 2);

        return scriptPubKey;
    }

    private static byte[] ConvertBits(ReadOnlySpan<byte> data, int fromBits, int toBits, bool pad)
    {
        var result = new List<byte>();
        int accumulator = 0;
        int bits = 0;
        int maxValue = (1 << toBits) - 1;
        int maxAccumulator = (1 << fromBits + toBits - 1) - 1;

        foreach (byte value in data)
        {
            if (value >> fromBits != 0)
            {
                throw new ArgumentException("The SegWit data contains an out-of-range value.", nameof(data));
            }

            accumulator = (accumulator << fromBits | value) & maxAccumulator;
            bits += fromBits;

            while (bits >= toBits)
            {
                bits -= toBits;
                result.Add((byte)(accumulator >> bits & maxValue));
            }
        }

        if (pad)
        {
            if (bits > 0)
            {
                result.Add((byte)(accumulator << toBits - bits & maxValue));
            }
        }
        else if (bits >= fromBits || (accumulator << toBits - bits & maxValue) != 0)
        {
            throw new ArgumentException("The SegWit witness program has invalid padding.", nameof(data));
        }

        return [.. result];
    }

    private static uint Bech32Polymod(ReadOnlySpan<byte> expandedHumanReadablePart, IReadOnlyList<byte> data)
    {
        ReadOnlySpan<uint> generators = [0x3b6a57b2, 0x26508e6d, 0x1ea119fa, 0x3d4233dd, 0x2a1462b3];
        uint checksum = 1;

        foreach (byte value in expandedHumanReadablePart)
        {
            checksum = UpdateBech32Checksum(checksum, value, generators);
        }

        foreach (byte value in data)
        {
            checksum = UpdateBech32Checksum(checksum, value, generators);
        }

        return checksum;
    }

    private static uint UpdateBech32Checksum(uint checksum, byte value, ReadOnlySpan<uint> generators)
    {
        uint top = checksum >> 25;
        checksum = (checksum & 0x1ffffff) << 5 ^ value;

        for (int i = 0; i < generators.Length; i++)
        {
            if ((top >> i & 1) != 0)
            {
                checksum ^= generators[i];
            }
        }

        return checksum;
    }

    private static byte[] ExpandHumanReadablePart(string humanReadablePart)
    {
        byte[] expanded = new byte[humanReadablePart.Length * 2 + 1];
        for (int i = 0; i < humanReadablePart.Length; i++)
        {
            expanded[i] = (byte)(humanReadablePart[i] >> 5);
            expanded[i + humanReadablePart.Length + 1] = (byte)(humanReadablePart[i] & 31);
        }

        return expanded;
    }
}
