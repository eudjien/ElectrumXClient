namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;

/// <summary>The current status of a subscribed master node.</summary>
public sealed class ElectrumXMasterNodeSubscription : ElectrumXResult
{
    /// <summary>Creates a result containing the current masternode subscription status.</summary>
    public ElectrumXMasterNodeSubscription(string? value)
    {
        Value = value;
    }

    /// <summary>Current server-reported masternode status, or <see langword="null" /> when the server returned no status.</summary>
    public string? Value { get; }

    public static implicit operator string?(ElectrumXMasterNodeSubscription response)
    {
        return response.Value;
    }
}
