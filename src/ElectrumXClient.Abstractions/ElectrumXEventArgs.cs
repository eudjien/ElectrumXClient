using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using System;

namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>Base type for all typed ElectrumX notification event payloads.</summary>
public abstract class ElectrumXNotificationEventArgs : EventArgs
{
    /// <summary>Initializes a new instance of the <see cref="ElectrumXNotificationEventArgs" /> class.</summary>
    protected ElectrumXNotificationEventArgs()
    {
    }
}

/// <summary>Base type for typed ElectrumX notification event payloads.</summary>
public abstract class ElectrumXNotificationEventArgs<TNotification> : ElectrumXNotificationEventArgs
{
    /// <summary>Initializes a new instance of the <see cref="ElectrumXNotificationEventArgs{TParameters}" /> class.</summary>
    protected ElectrumXNotificationEventArgs(TNotification notification)
    {
        Notification = notification;
    }

    /// <summary>The notification payload associated with the event.</summary>
    public TNotification Notification { get; }
}

/// <summary>Event data for a new block header notification.</summary>
public sealed class ElectrumXBlockHeaderEventArgs(ElectrumXHeadersNotification notification)
    : ElectrumXNotificationEventArgs<ElectrumXHeadersNotification>(notification);

/// <summary>Event data for a new block-height notification from protocol version 1.0.</summary>
public sealed class ElectrumXBlockHeightEventArgs(ElectrumXBlockHeightNotification notification)
    : ElectrumXNotificationEventArgs<ElectrumXBlockHeightNotification>(notification);

/// <summary>Event data for a script hash status notification.</summary>
public sealed class ElectrumXScriptHashStatusEventArgs(ElectrumXScriptHashNotification notification)
    : ElectrumXNotificationEventArgs<ElectrumXScriptHashNotification>(notification);

/// <summary>Event data for a masterNode status notification.</summary>
public sealed class ElectrumXMasterNodeStatusEventArgs(ElectrumXMasterNodeNotification notification)
    : ElectrumXNotificationEventArgs<ElectrumXMasterNodeNotification>(notification);

/// <summary>Event data for a blockchain address status notification.</summary>
public sealed class ElectrumXAddressNotificationEventArgs(
    ElectrumXAddressNotification notification)
    : ElectrumXNotificationEventArgs<ElectrumXAddressNotification>(notification);

/// <summary>Event data for a blockchain transaction notification.</summary>
public sealed class ElectrumXTransactionNotificationEventArgs(
    ElectrumXTransactionNotification notification)
    : ElectrumXNotificationEventArgs<ElectrumXTransactionNotification>(notification);

/// <summary>Event data for a double-spend-proof notification.</summary>
public sealed class ElectrumXDoubleSpendProofNotificationEventArgs(
    ElectrumXDoubleSpendProofNotification notification)
    : ElectrumXNotificationEventArgs<ElectrumXDoubleSpendProofNotification>(notification);
