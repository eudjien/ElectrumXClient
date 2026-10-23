namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;

/// <summary>The banner message returned by the server.</summary>
public sealed class ElectrumXBanner : ElectrumXResult
{
    /// <summary>Creates a result containing the server banner.</summary>
    public ElectrumXBanner(string value)
    {
        Value = value;
    }

    /// <summary>Free-form server banner text returned by <c>server.banner</c>.</summary>
    public string Value { get; }

    public static implicit operator string(ElectrumXBanner response)
    {
        return response.Value;
    }
}
