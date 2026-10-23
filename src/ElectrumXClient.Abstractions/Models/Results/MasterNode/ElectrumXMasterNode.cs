namespace NorthGomel.ElectrumXClient.Abstractions.Models.Results.MasterNode;

/// <summary>A masterNode entry returned by the Dash-compatible masterNode.list method.</summary>
public sealed class ElectrumXMasterNode : ElectrumXResult
{
    /// <summary>Creates a Dash-compatible masternode entry; fields vary by server implementation.</summary>
    public ElectrumXMasterNode(string? vin = null, string? status = null, int? protocol = null, string? payee = null, string? lastSeen = null,
        long? activeSeconds = null, string? lastPaidTime = null, long? lastPaidBlock = null, string? ip = null, long? paymentPosition = null,
        bool? inSelection = null, decimal? balance = null)
    {
        Vin = vin;
        Status = status;
        Protocol = protocol;
        Payee = payee;
        LastSeen = lastSeen;
        ActiveSeconds = activeSeconds;
        LastPaidTime = lastPaidTime;
        LastPaidBlock = lastPaidBlock;
        Ip = ip;
        PaymentPosition = paymentPosition;
        InSelection = inSelection;
        Balance = balance;
    }

    /// <summary>Server-formatted transaction outpoint identifying the masternode collateral.</summary>
    public string? Vin { get; }

    /// <summary>Server-reported masternode status.</summary>
    public string? Status { get; }

    /// <summary>Masternode protocol version.</summary>
    public int? Protocol { get; }

    /// <summary>Chain-specific address receiving masternode payments (typically Base58 on Dash mainnet).</summary>
    public string? Payee { get; }

    /// <summary>Server-formatted timestamp for when the masternode was last seen; format is server-dependent.</summary>
    public string? LastSeen { get; }

    /// <summary>Number of seconds the masternode has been active.</summary>
    public long? ActiveSeconds { get; }

    /// <summary>Server-formatted timestamp for when the masternode was last paid; format is server-dependent.</summary>
    public string? LastPaidTime { get; }

    /// <summary>Block height at which the masternode was last paid.</summary>
    public long? LastPaidBlock { get; }

    /// <summary>Masternode IP address and port in <c>host:port</c> notation.</summary>
    public string? Ip { get; }

    /// <summary>Position in the server's payment queue.</summary>
    public long? PaymentPosition { get; }

    /// <summary>Whether the masternode is currently selected for payment.</summary>
    public bool? InSelection { get; }

    /// <summary>Collateral balance in whole coin units as reported by the server.</summary>
    public decimal? Balance { get; }
}
