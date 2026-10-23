using System;

namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>Validated minimum and maximum Electrum protocol versions.</summary>
public sealed class ElectrumXProtocolVersionLimits
{

    /// <summary>Creates protocol version limits using the default range.</summary>
    public ElectrumXProtocolVersionLimits()
        : this(MinimumDefault, MaximumDefault)
    {
    }

    /// <summary>Creates protocol version limits from versions with up to three components.</summary>
    /// <param name="minimum">The minimum protocol version.</param>
    /// <param name="maximum">The maximum protocol version.</param>
    public ElectrumXProtocolVersionLimits(ElectrumXProtocolVersion minimum, ElectrumXProtocolVersion maximum)
    {
        ArgumentGuard.ThrowIfNull(minimum, nameof(minimum));
        ArgumentGuard.ThrowIfNull(maximum, nameof(maximum));

        if (minimum > maximum)
        {
            throw new ArgumentException("Protocol minimum must not exceed protocol maximum.", nameof(minimum));
        }

        Minimum = minimum;
        Maximum = maximum;
    }
    public static ElectrumXProtocolVersion MinimumDefault { get; } = new(1, 4);

    public static ElectrumXProtocolVersion MaximumDefault { get; } = new(1, 6);

    public ElectrumXProtocolVersion Minimum { get; }

    public ElectrumXProtocolVersion Maximum { get; }
}
