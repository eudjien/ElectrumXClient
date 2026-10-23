using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Exceptions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Notifications;
using System;

namespace NorthGomel.ElectrumXClient.JsonRpc;

internal sealed class ElectrumXNotificationDispatcher
{
    private readonly IElectrumXJsonConverter _converter;

    internal ElectrumXNotificationDispatcher(IElectrumXJsonConverter converter)
    {
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
    }

    internal event EventHandler<ElectrumXBlockHeaderEventArgs>? BlockHeaderReceived;

    internal event EventHandler<ElectrumXBlockHeightEventArgs>? BlockHeightReceived;

    internal event EventHandler<ElectrumXScriptHashStatusEventArgs>? ScriptHashStatusChanged;

    internal event EventHandler<ElectrumXMasterNodeStatusEventArgs>? MasterNodeStatusChanged;

    internal event EventHandler<ElectrumXAddressNotificationEventArgs>? AddressNotificationReceived;

    internal event EventHandler<ElectrumXTransactionNotificationEventArgs>? TransactionNotificationReceived;

    internal event EventHandler<ElectrumXDoubleSpendProofNotificationEventArgs>? DoubleSpendProofNotificationReceived;

    internal void Dispatch(string? method, ReadOnlyMemory<byte> message)
    {
        if (string.IsNullOrWhiteSpace(method))
        {
            return;
        }

        switch (method)
        {
            case ElectrumXJsonMethodNames.Blockchain.HeadersSubscribe:

                JsonRpcNotification<ElectrumXHeadersNotification> headersNotification;

                try
                {
                    headersNotification = _converter.DeserializeNotification<ElectrumXHeadersNotification>(message);
                }
                catch (Exception exception)
                {
                    throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC notification.", exception);
                }

                ValidateNotificationVersion(headersNotification.JsonRpcVersion);

                var headerParameters = headersNotification.Parameters ?? throw new ElectrumXProtocolException(
                    "The {0} notification payload was null.",
                    "block-header");

                ElectrumXNotificationPublisher.Publish(BlockHeaderReceived, this, new ElectrumXBlockHeaderEventArgs(headerParameters));

                break;

            case ElectrumXJsonMethodNames.Blockchain.NumBlocks.Subscribe:

                JsonRpcNotification<ElectrumXBlockHeightNotification> blockHeightNotification;

                try
                {
                    blockHeightNotification = _converter.DeserializeNotification<ElectrumXBlockHeightNotification>(message);
                }
                catch (Exception exception)
                {
                    throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC notification.", exception);
                }

                ValidateNotificationVersion(blockHeightNotification.JsonRpcVersion);

                var blockHeightParameters = blockHeightNotification.Parameters
                                            ?? throw new ElectrumXProtocolException(
                                                "The {0} notification payload was null.",
                                                "block-height");

                ElectrumXNotificationPublisher.Publish(
                    BlockHeightReceived,
                    this,
                    new ElectrumXBlockHeightEventArgs(blockHeightParameters));

                break;

            case ElectrumXJsonMethodNames.Blockchain.ScriptHash.Subscribe:

                JsonRpcNotification<ElectrumXScriptHashNotification> scriptHashNotification;

                try
                {
                    scriptHashNotification = _converter.DeserializeNotification<ElectrumXScriptHashNotification>(message);
                }
                catch (Exception exception)
                {
                    throw new ElectrumXProtocolException(
                        "The ElectrumX server sent a malformed JSON-RPC notification.",
                        exception);
                }

                ValidateNotificationVersion(scriptHashNotification.JsonRpcVersion);

                var scriptHashParameters = scriptHashNotification.Parameters
                                           ?? throw new ElectrumXProtocolException(
                                               "The {0} notification payload was null.",
                                               "script-hash");

                ElectrumXNotificationPublisher.Publish(
                    ScriptHashStatusChanged,
                    this,
                    new ElectrumXScriptHashStatusEventArgs(
                        new ElectrumXScriptHashNotification(scriptHashParameters.ScriptHash, scriptHashParameters.Status)));

                break;

            case ElectrumXJsonMethodNames.MasterNode.Subscribe:

                JsonRpcNotification<ElectrumXMasterNodeNotification> masterNodeNotification;

                try
                {
                    masterNodeNotification = _converter.DeserializeNotification<ElectrumXMasterNodeNotification>(message);
                }
                catch (Exception exception)
                {
                    throw new ElectrumXProtocolException(
                        "The ElectrumX server sent a malformed JSON-RPC notification.",
                        exception);
                }

                ValidateNotificationVersion(masterNodeNotification.JsonRpcVersion);

                var masterNodeParameters = masterNodeNotification.Parameters
                                           ?? throw new ElectrumXProtocolException(
                                               "The {0} notification payload was null.",
                                               "masterNode");

                if (string.IsNullOrWhiteSpace(masterNodeParameters.Collateral))
                {
                    throw new ElectrumXProtocolException(
                        "Notification '{0}' contains a missing or invalid collateral.",
                        masterNodeNotification.Method);
                }

                ElectrumXNotificationPublisher.Publish(
                    MasterNodeStatusChanged,
                    this,
                    new ElectrumXMasterNodeStatusEventArgs(masterNodeParameters));

                break;

            case ElectrumXJsonMethodNames.Blockchain.Address.Subscribe:
                DispatchBlockchainNotification<ElectrumXAddressNotification>(
                    message,
                    notification => ElectrumXNotificationPublisher.Publish(
                        AddressNotificationReceived,
                        this,
                        new ElectrumXAddressNotificationEventArgs(notification)));

                break;

            case ElectrumXJsonMethodNames.Blockchain.Transaction.Subscribe:
                DispatchBlockchainNotification<ElectrumXTransactionNotification>(
                    message,
                    notification => ElectrumXNotificationPublisher.Publish(
                        TransactionNotificationReceived,
                        this,
                        new ElectrumXTransactionNotificationEventArgs(notification)));

                break;

            case ElectrumXJsonMethodNames.Blockchain.Transaction.DsProof.Subscribe:
                DispatchBlockchainNotification<ElectrumXDoubleSpendProofNotification>(
                    message,
                    notification => ElectrumXNotificationPublisher.Publish(
                        DoubleSpendProofNotificationReceived,
                        this,
                        new ElectrumXDoubleSpendProofNotificationEventArgs(notification)));

                break;
        }
    }

    private void DispatchBlockchainNotification<TNotification>(
        ReadOnlyMemory<byte> message,
        Action<TNotification> publish)
        where TNotification : ElectrumXNotification
    {
        JsonRpcNotification<TNotification> notification;

        try
        {
            notification = _converter.DeserializeNotification<TNotification>(message);
        }
        catch (Exception exception)
        {
            throw new ElectrumXProtocolException("The ElectrumX server sent a malformed JSON-RPC notification.", exception);
        }

        ValidateNotificationVersion(notification.JsonRpcVersion);

        var parameters = notification.Parameters
                         ?? throw new ElectrumXProtocolException(
                             "Notification '{0}' contained no parameters.",
                             notification.Method);

        publish(parameters);
    }

    private static void ValidateNotificationVersion(string version)
    {
        if (version != ElectrumXJsonPropertyValues.JsonRpcVersion)
        {
            throw new ElectrumXProtocolException(
                "Unsupported JSON-RPC version '{0}'. Expected '{1}'.",
                version,
                ElectrumXJsonPropertyValues.JsonRpcVersion);
        }
    }
}
