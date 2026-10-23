namespace NorthGomel.ElectrumXClient.Abstractions;

public static class ElectrumXJsonPropertyNames
{
    public const string JsonRpcVersion = "jsonrpc";

    public static class JsonRpcRequest
    {
        public const string Id = "id";
        public const string Method = "method";
        public const string Parameters = "params";
    }

    public static class JsonRpcResponse
    {
        public const string Id = "id";
        public const string Method = "method";
        public const string Result = "result";
        public const string Error = "error";
    }

    public static class JsonRpcNotification
    {
        public const string Method = "method";
        public const string Parameters = "params";
    }

    public static class DaemonPassthroughQuery
    {
        public const string Method = "method";
        public const string Parameters = "params";
    }

    public static class BalanceResult
    {
        public const string Confirmed = "confirmed";
        public const string Unconfirmed = "unconfirmed";
    }

    public static class BlockHeaderSubscriptionResult
    {
        public const string Height = "height";
        public const string Hex = "hex";
    }

    public static class DeserializedBlockHeader
    {
        public const string BlockHeight = "block_height";
    }

    public static class DoubleSpendProofResult
    {
        public const string DoubleSpendProofId = "dspid";
        public const string TransactionId = "txid";
        public const string Hex = "hex";
        public const string Outpoint = "outpoint";
        public const string Descendants = "descendants";
        public const string OutputIndex = "vout";
    }

    public static class FirstUseResult
    {
        public const string BlockHash = "block_hash";
        public const string Height = "height";
        public const string TransactionHash = "tx_hash";
    }

    public static class UtxoInfoResult
    {
        public const string ConfirmedHeight = "confirmed_height";
        public const string ScriptHash = "scripthash";
        public const string Value = "value";
        public const string TokenData = "token_data";
    }

    public static class ConfirmedTransactionBlockHashResult
    {
        public const string BlockHash = "block_hash";
        public const string BlockHeight = "block_height";
        public const string BlockHeader = "block_header";
    }

    public static class HeaderInfoResult
    {
        public const string Height = "height";
        public const string Hex = "hex";
    }

    public static class MempoolInfoResult
    {
        public const string MempoolMinimumFee = "mempoolminfee";
        public const string MinimumRelayFee = "minrelaytxfee";
        public const string IncrementalRelayFee = "incrementalrelayfee";
        public const string UnbroadcastCount = "unbroadcastcount";
        public const string FullReplaceByFee = "fullrbf";
    }

    public static class HeadersNotificationParameters
    {
        public const string Height = "height";
        public const string Hex = "hex";
    }

    public static class HeaderProofResult
    {
        public const string Branch = "branch";
        public const string Header = "header";
        public const string Root = "root";
    }

    public static class HeadersResult
    {
        public const string Count = "count";
        public const string Hex = "hex";
        public const string Headers = "headers";
        public const string Maximum = "max";
        public const string Root = "root";
        public const string Branch = "branch";
    }

    public static class HistoryEntryResult
    {
        public const string Height = "height";
        public const string TransactionHash = "tx_hash";
        public const string Fee = "fee";
    }

    public static class IdFromPositionResult
    {
        public const string TransactionHash = "tx_hash";
        public const string Merkle = "merkle";
    }

    public static class MasterNodeResult
    {
        public const string Vin = "vin";
        public const string Status = "status";
        public const string Protocol = "protocol";
        public const string Payee = "payee";
        public const string LastSeen = "lastseen";
        public const string ActiveSeconds = "activeseconds";
        public const string LastPaidTime = "lastpaidtime";
        public const string LastPaidBlock = "lastpaidblock";
        public const string Ip = "ip";
        public const string PaymentPosition = "paymentposition";
        public const string InSelection = "inselection";
        public const string Balance = "balance";
    }

    public static class MempoolEntryResult
    {
        public const string Height = "height";
        public const string TransactionHash = "tx_hash";
        public const string Fee = "fee";
    }

    public static class ProTxDiffResult
    {
        public const string BaseBlockHash = "baseBlockHash";
        public const string BlockHash = "blockHash";
        public const string CoinbaseTransactionMerkleTree = "cbTxMerkleTree";
        public const string CoinbaseTransaction = "cbTx";
        public const string DeletedMasterNodes = "deletedMNs";
        public const string MasterNodes = "mnList";
        public const string MasterNodeListMerkleRoot = "merkleRootMNList";
    }

    public static class ProTxInfoResult
    {
        public const string ProTxHash = "proTxHash";
        public const string CollateralHash = "collateralHash";
        public const string CollateralIndex = "collateralIndex";
        public const string OperatorReward = "operatorReward";
        public const string State = "state";
        public const string Confirmations = "confirmations";
    }

    public static class ProTxMasterNodeResult
    {
        public const string ProRegTxHash = "proRegTxHash";
        public const string ConfirmedHash = "confirmedHash";
        public const string Service = "service";
        public const string OperatorPublicKey = "pubKeyOperator";
        public const string VotingKeyId = "keyIDVoting";
        public const string IsValid = "isValid";
    }

    public static class ProTxStateResult
    {
        public const string RegisteredHeight = "registeredHeight";
        public const string LastPaidHeight = "lastPaidHeight";
        public const string PoSePenalty = "PoSePenalty";
        public const string PoSeRevivedHeight = "PoSeRevivedHeight";
        public const string PoSeBanHeight = "PoSeBanHeight";
        public const string RevocationReason = "revocationReason";
        public const string OwnerKeyId = "keyIDOwner";
        public const string OperatorPublicKey = "pubKeyOperator";
        public const string VotingKeyId = "keyIDVoting";
        public const string OwnerKeyAddress = "ownerKeyAddr";
        public const string VotingKeyAddress = "votingKeyAddr";
        public const string Address = "addr";
        public const string PayoutAddress = "payoutAddress";
    }

    public static class RpcErrorResult
    {
        public const string Code = "code";
        public const string Message = "message";
        public const string Data = "data";
    }

    public static class ServerFeaturesResult
    {
        public const string Hosts = "hosts";
        public const string GenesisHash = "genesis_hash";
        public const string HashFunction = "hash_function";
        public const string ServerVersion = "server_version";
        public const string ProtocolMinimum = "protocol_min";
        public const string ProtocolMaximum = "protocol_max";
        public const string Pruning = "pruning";
        public const string DoubleSpendProof = "dsproof";
        public const string CashTokens = "cashtokens";
        public const string Rpa = "rpa";
        public const string BroadcastPackage = "broadcast_package";
    }

    public static class ServerHost
    {
        public const string SslPort = "ssl_port";
        public const string TcpPort = "tcp_port";
        public const string WebSocketPort = "ws_port";
        public const string WebSocketSecurePort = "wss_port";
    }

    public static class TransactionMerkleResult
    {
        public const string BlockHeight = "block_height";
        public const string Merkle = "merkle";
        public const string Position = "pos";
    }

    public static class TscMerkleResult
    {
        public const string Composite = "composite";
        public const string Index = "index";
        public const string Nodes = "nodes";
        public const string ProofType = "proofType";
        public const string Target = "target";
        public const string TargetType = "targetType";
        public const string TransactionOrId = "txOrId";
    }

    public static class UnspentOutputResult
    {
        public const string Height = "height";
        public const string TransactionPosition = "tx_pos";
        public const string TransactionHash = "tx_hash";
        public const string Value = "value";
        public const string TokenData = "token_data";
    }

    public static class TokenDataResult
    {
        public const string Amount = "amount";
        public const string Category = "category";
        public const string Nft = "nft";
        public const string Capability = "capability";
        public const string Commitment = "commitment";
        public const string CapabilityNone = "none";
        public const string CapabilityMutable = "mutable";
        public const string CapabilityMinting = "minting";
    }

    public static class VerboseScriptSigResult
    {
        public const string Assembly = "asm";
        public const string Hex = "hex";
    }

    public static class VerboseScriptPubKeyResult
    {
        public const string Assembly = "asm";
        public const string Hex = "hex";
        public const string RequiredSignatures = "reqSigs";
        public const string Type = "type";
        public const string Addresses = "addresses";
    }

    public static class VerboseTransactionInputResult
    {
        public const string TransactionId = "txid";
        public const string OutputIndex = "vout";
        public const string ScriptSignature = "scriptSig";
        public const string Sequence = "sequence";
        public const string Coinbase = "coinbase";
    }

    public static class VerboseTransactionResult
    {
        public const string BlockHash = "blockhash";
        public const string BlockTime = "blocktime";
        public const string Confirmations = "confirmations";
        public const string Hash = "hash";
        public const string Hex = "hex";
        public const string LockTime = "locktime";
        public const string Size = "size";
        public const string Time = "time";
        public const string TransactionId = "txid";
        public const string Version = "version";
        public const string Inputs = "vin";
        public const string Outputs = "vout";
    }

    public static class VerboseTransactionOutputResult
    {
        public const string Index = "n";
        public const string ScriptPubKey = "scriptPubKey";
        public const string Value = "value";
    }
}
