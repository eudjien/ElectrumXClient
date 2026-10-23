using NorthGomel.ElectrumXClient.Converters.JsonRpc;
using NorthGomel.ElectrumXClient.Converters.Notifications;
using NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;
using NorthGomel.ElectrumXClient.Converters.Queries.MasterNode;
using NorthGomel.ElectrumXClient.Converters.Queries.Mempool;
using NorthGomel.ElectrumXClient.Converters.Queries.ProTx;
using NorthGomel.ElectrumXClient.Converters.Queries.Server;
using NorthGomel.ElectrumXClient.Converters.Results;
using NorthGomel.ElectrumXClient.Converters.Results.Blockchain;
using NorthGomel.ElectrumXClient.Converters.Results.MasterNode;
using NorthGomel.ElectrumXClient.Converters.Results.Mempool;
using NorthGomel.ElectrumXClient.Converters.Results.ProTx;
using NorthGomel.ElectrumXClient.Converters.Results.Server;
using System;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters;

/// <summary>Extensions for registering ElectrumX converters with System.Text.Json.</summary>
public static class SystemTextJsonConverterExtensions
{
    extension(JsonSerializerOptions options)
    {
        public JsonSerializerOptions AddElectrumXConverters()
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.AddElectrumXJsonRpcConverters();
            options.AddElectrumXQueryConverters();
            options.AddElectrumXNotificationConverters();
            options.AddElectrumXResultConverters();

            return options;
        }

        public JsonSerializerOptions AddElectrumXQueryConverters()
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Converters.Add(new BlockHeadersQueryJsonConverter());
            options.Converters.Add(new AddressBalanceQueryJsonConverter());
            options.Converters.Add(new AddressHistoryQueryJsonConverter());
            options.Converters.Add(new AddressMempoolQueryJsonConverter());
            options.Converters.Add(new AddressListUnspentQueryJsonConverter());
            options.Converters.Add(new AddressScriptHashQueryJsonConverter());
            options.Converters.Add(new AddressFirstUseQueryJsonConverter());
            options.Converters.Add(new AddressStatusQueryJsonConverter());
            options.Converters.Add(new SubscribeToAddressQueryJsonConverter());
            options.Converters.Add(new UnsubscribeFromAddressQueryJsonConverter());
            options.Converters.Add(new UnspentOutputInfoQueryJsonConverter());
            options.Converters.Add(new TransactionHeightQueryJsonConverter());
            options.Converters.Add(new SubscribeToTransactionQueryJsonConverter());
            options.Converters.Add(new UnsubscribeFromTransactionQueryJsonConverter());
            options.Converters.Add(new GetDoubleSpendProofQueryJsonConverter());
            options.Converters.Add(new ListDoubleSpendProofTransactionsQueryJsonConverter());
            options.Converters.Add(new SubscribeToDoubleSpendProofQueryJsonConverter());
            options.Converters.Add(new UnsubscribeFromDoubleSpendProofQueryJsonConverter());
            options.Converters.Add(new ConfirmedTransactionBlockHashQueryJsonConverter());
            options.Converters.Add(new HeaderByHashQueryJsonConverter());
            options.Converters.Add(new HeaderByHeightQueryJsonConverter());
            options.Converters.Add(new GetHeaderTipQueryJsonConverter());
            options.Converters.Add(new UnsubscribeFromHeadersQueryJsonConverter());
            options.Converters.Add(new ScriptHashStatusQueryJsonConverter());
            options.Converters.Add(new ScriptHashFirstUseQueryJsonConverter());
            options.Converters.Add(new RpaHistoryQueryJsonConverter());
            options.Converters.Add(new RpaMempoolQueryJsonConverter());
            options.Converters.Add(new ReusableHistoryQueryJsonConverter());
            options.Converters.Add(new ReusableMempoolQueryJsonConverter());
            options.Converters.Add(new BroadcastPackageQueryJsonConverter());
            options.Converters.Add(new LegacyBlockHeaderQueryJsonConverter());
            options.Converters.Add(new BlockHeaderChunkQueryJsonConverter());
            options.Converters.Add(new NumBlocksSubscribeQueryJsonConverter());
            options.Converters.Add(new UtxoAddressQueryJsonConverter());
            options.Converters.Add(new MempoolInfoQueryJsonConverter());
            options.Converters.Add(new EstimateFeeQueryJsonConverter());
            options.Converters.Add(new SubscribeToHeadersQueryJsonConverter());
            options.Converters.Add(new RelayFeeQueryJsonConverter());
            options.Converters.Add(new BalanceQueryJsonConverter());
            options.Converters.Add(new HistoryQueryJsonConverter());
            options.Converters.Add(new MempoolQueryJsonConverter());
            options.Converters.Add(new ListUnspentQueryJsonConverter());
            options.Converters.Add(new BlockHeaderQueryJsonConverter());
            options.Converters.Add(new BlockHeaderWithProofQueryJsonConverter());
            options.Converters.Add(new SubscribeToScriptHashQueryJsonConverter());
            options.Converters.Add(new UnsubscribeFromScriptHashQueryJsonConverter());
            options.Converters.Add(new RawTransactionQueryJsonConverter());
            options.Converters.Add(new VerboseTransactionQueryJsonConverter());
            options.Converters.Add(new BroadcastTransactionQueryJsonConverter());
            options.Converters.Add(new TransactionMerkleQueryJsonConverter());
            options.Converters.Add(new TscMerkleWithTransactionHashQueryJsonConverter());
            options.Converters.Add(new TscMerkleWithTransactionHexQueryJsonConverter());
            options.Converters.Add(new TransactionIdFromPositionQueryJsonConverter());
            options.Converters.Add(new BroadcastMasterNodeAnnouncementQueryJsonConverter());
            options.Converters.Add(new SubscribeToMasterNodeQueryJsonConverter());
            options.Converters.Add(new MasterNodesQueryJsonConverter());
            options.Converters.Add(new FeeHistogramQueryJsonConverter());
            options.Converters.Add(new ProTxDiffQueryJsonConverter());
            options.Converters.Add(new ProTxInfoQueryJsonConverter());
            options.Converters.Add(new AddPeerQueryJsonConverter());
            options.Converters.Add(new BannerQueryJsonConverter());
            options.Converters.Add(new DonationAddressQueryJsonConverter());
            options.Converters.Add(new ServerFeaturesQueryJsonConverter());
            options.Converters.Add(new PeersQueryJsonConverter());
            options.Converters.Add(new PingQueryJsonConverter());
            options.Converters.Add(new ServerVersionQueryJsonConverter());
            options.Converters.Add(new DaemonPassthroughQueryJsonConverter());

            return options;
        }

        public JsonSerializerOptions AddElectrumXJsonRpcConverters()
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Converters.Add(new JsonRpcRequestConverterJsonFactory());
            options.Converters.Add(new JsonRpcHeaderJsonConverter());
            options.Converters.Add(new JsonRpcResponseJsonConverterFactory());
            options.Converters.Add(new JsonRpcErrorJsonConverter());

            return options;
        }

        public JsonSerializerOptions AddElectrumXNotificationConverters()
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Converters.Add(new JsonRpcNotificationJsonConverterFactory());
            options.Converters.Add(new HeadersNotificationJsonConverter());
            options.Converters.Add(new BlockHeightNotificationJsonConverter());
            options.Converters.Add(new MasterNodeNotificationJsonConverter());
            options.Converters.Add(new ScriptHashSubscribeNotificationJsonConverter());
            options.Converters.Add(new AddressNotificationJsonConverter());
            options.Converters.Add(new TransactionNotificationJsonConverter());
            options.Converters.Add(new DoubleSpendProofNotificationJsonConverter());

            return options;
        }

        public JsonSerializerOptions AddElectrumXResultConverters()
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Converters.Add(new ElectrumXJsonValueResultJsonConverter());
            options.Converters.Add(new ElectrumXJsonValueEnvelopeResultJsonConverter());
            options.Converters.Add(new AddressScriptHashResultJsonConverter());
            options.Converters.Add(new AddressStatusResultJsonConverter());
            options.Converters.Add(new AddressSubscriptionResultJsonConverter());
            options.Converters.Add(new ScriptHashStatusResultJsonConverter());
            options.Converters.Add(new AddressUnsubscriptionResultJsonConverter());
            options.Converters.Add(new TransactionUnsubscriptionResultJsonConverter());
            options.Converters.Add(new DoubleSpendProofUnsubscriptionResultJsonConverter());
            options.Converters.Add(new HeadersUnsubscriptionResultJsonConverter());
            options.Converters.Add(new TransactionHeightResultJsonConverter());
            options.Converters.Add(new TransactionSubscriptionResultJsonConverter());
            options.Converters.Add(new StringListResultJsonConverter());
            options.Converters.Add(new HistoryListResultJsonConverter());
            options.Converters.Add(new MempoolEntryListResultJsonConverter());
            options.Converters.Add(new ServerHostPortsResultJsonConverter());
            options.Converters.Add(new AddPeerResultJsonConverter());
            options.Converters.Add(new BalanceResultJsonConverter());
            options.Converters.Add(new BannerResultJsonConverter());
            options.Converters.Add(new BlockHeaderSubscriptionResultJsonConverter());
            options.Converters.Add(new BlockHeaderChunkResultJsonConverter());
            options.Converters.Add(new BlockHeightResultJsonConverter());
            options.Converters.Add(new BlockHeaderResultJsonConverter());
            options.Converters.Add(new ConfirmedTransactionBlockHashResultJsonConverter());
            options.Converters.Add(new BroadcastMasterNodeAnnouncementResultJsonConverter());
            options.Converters.Add(new BroadcastTransactionResultJsonConverter());
            options.Converters.Add(new UtxoAddressResultJsonConverter());
            options.Converters.Add(new DonationAddressResultJsonConverter());
            options.Converters.Add(new DoubleSpendProofResultJsonConverter());
            options.Converters.Add(new EstimateFeeResultJsonConverter());
            options.Converters.Add(new FirstUseResultJsonConverter());
            options.Converters.Add(new FeeHistogramEntryResultJsonConverter());
            options.Converters.Add(new FeeHistogramResultJsonConverter());
            options.Converters.Add(new HeaderProofResultJsonConverter());
            options.Converters.Add(new HeaderInfoResultJsonConverter());
            options.Converters.Add(new HeadersResultJsonConverter());
            options.Converters.Add(new HistoryEntryResultJsonConverter());
            options.Converters.Add(new HistoryResultJsonConverter());
            options.Converters.Add(new IdFromPositionResultJsonConverter());
            options.Converters.Add(new MasterNodeResultJsonConverter());
            options.Converters.Add(new MasterNodeSubscriptionResultJsonConverter());
            options.Converters.Add(new MasterNodesResultJsonConverter());
            options.Converters.Add(new MempoolEntryResultJsonConverter());
            options.Converters.Add(new MempoolInfoResultJsonConverter());
            options.Converters.Add(new MempoolResultJsonConverter());
            options.Converters.Add(new PeerResultJsonConverter());
            options.Converters.Add(new PeersResultJsonConverter());
            options.Converters.Add(new PingResultJsonConverter());
            options.Converters.Add(new ProTxDiffResultJsonConverter());
            options.Converters.Add(new ProTxInfoResultJsonConverter());
            options.Converters.Add(new ProTxMasterNodeResultJsonConverter());
            options.Converters.Add(new ProTxStateResultJsonConverter());
            options.Converters.Add(new RawTransactionResultJsonConverter());
            options.Converters.Add(new RelayFeeResultJsonConverter());
            options.Converters.Add(new ServerFeaturesResultJsonConverter());
            options.Converters.Add(new ServerVersionResultJsonConverter());
            options.Converters.Add(new ScriptHashSubscriptionResultJsonConverter());
            options.Converters.Add(new ScriptHashUnsubscriptionResultJsonConverter());
            options.Converters.Add(new TransactionMerkleResultJsonConverter());
            options.Converters.Add(new TscMerkleWithTransactionHashResultJsonConverter());
            options.Converters.Add(new TscMerkleWithTransactionHexResultJsonConverter());
            options.Converters.Add(new UnspentOutputResultJsonConverter());
            options.Converters.Add(new UnspentOutputsResultJsonConverter());
            options.Converters.Add(new UtxoInfoResultJsonConverter());
            options.Converters.Add(new VerboseScriptSigResultJsonConverter());
            options.Converters.Add(new VerboseScriptPubKeyResultJsonConverter());
            options.Converters.Add(new VerboseTransactionResultJsonConverter());
            options.Converters.Add(new VerboseTransactionInputResultJsonConverter());
            options.Converters.Add(new VerboseTransactionOutputResultJsonConverter());

            return options;
        }
    }
}
