// ReSharper disable MemberHidesStaticFromOuterClass
namespace NorthGomel.ElectrumXClient.Abstractions;

/// <summary>ElectrumX JSON-RPC method names used by the client.</summary>
public static class ElectrumXJsonMethodNames
{
    public static class Blockchain
    {
        public const string EstimateFee = "blockchain.estimatefee";
        public const string RelayFee = "blockchain.relayfee";
        public const string HeadersSubscribe = "blockchain.headers.subscribe";

        public static class NumBlocks
        {
            public const string Subscribe = "blockchain.numblocks.subscribe";
        }

        public static class Address
        {
            public const string GetBalance = "blockchain.address.get_balance";
            public const string GetFirstUse = "blockchain.address.get_first_use";
            public const string GetHistory = "blockchain.address.get_history";
            public const string GetMempool = "blockchain.address.get_mempool";
            public const string GetScriptHash = "blockchain.address.get_scripthash";
            public const string GetStatus = "blockchain.address.get_status";
            public const string ListUnspent = "blockchain.address.listunspent";
            public const string Subscribe = "blockchain.address.subscribe";
            public const string Unsubscribe = "blockchain.address.unsubscribe";
        }

        public static class Block
        {
            public const string GetChunk = "blockchain.block.get_chunk";
            public const string GetHeader = "blockchain.block.get_header";
            public const string Header = "blockchain.block.header";
            public const string Headers = "blockchain.block.headers";
        }

        public static class Header
        {
            public const string Get = "blockchain.header.get";
        }

        public static class Headers
        {
            public const string GetTip = "blockchain.headers.get_tip";
            public const string Unsubscribe = "blockchain.headers.unsubscribe";
        }

        public static class ScriptHash
        {
            public const string GetBalance = "blockchain.scripthash.get_balance";
            public const string GetFirstUse = "blockchain.scripthash.get_first_use";
            public const string GetHistory = "blockchain.scripthash.get_history";
            public const string GetMempool = "blockchain.scripthash.get_mempool";
            public const string GetStatus = "blockchain.scripthash.get_status";
            public const string ListUnspent = "blockchain.scripthash.listunspent";
            public const string Subscribe = "blockchain.scripthash.subscribe";
            public const string Unsubscribe = "blockchain.scripthash.unsubscribe";
        }

        public static class Rpa
        {
            public const string GetHistory = "blockchain.rpa.get_history";
            public const string GetMempool = "blockchain.rpa.get_mempool";
        }

        public static class Reusable
        {
            public const string GetHistory = "blockchain.reusable.get_history";
            public const string GetMempool = "blockchain.reusable.get_mempool";
        }

        public static class Transaction
        {
            public const string Broadcast = "blockchain.transaction.broadcast";
            public const string BroadcastPackage = "blockchain.transaction.broadcast_package";
            public const string Get = "blockchain.transaction.get";
            public const string GetConfirmedBlockHash = "blockchain.transaction.get_confirmed_blockhash";
            public const string GetHeight = "blockchain.transaction.get_height";
            public const string GetMerkle = "blockchain.transaction.get_merkle";
            public const string GetTscMerkle = "blockchain.transaction.get_tsc_merkle";
            public const string IdFromPosition = "blockchain.transaction.id_from_pos";
            public const string Subscribe = "blockchain.transaction.subscribe";
            public const string Unsubscribe = "blockchain.transaction.unsubscribe";

            public static class DsProof
            {
                public const string Get = "blockchain.transaction.dsproof.get";
                public const string List = "blockchain.transaction.dsproof.list";
                public const string Subscribe = "blockchain.transaction.dsproof.subscribe";
                public const string Unsubscribe = "blockchain.transaction.dsproof.unsubscribe";
            }
        }

        public static class Utxo
        {
            public const string GetAddress = "blockchain.utxo.get_address";
            public const string GetInfo = "blockchain.utxo.get_info";
        }
    }

    public static class Mempool
    {
        public const string GetFeeHistogram = "mempool.get_fee_histogram";
        public const string GetInfo = "mempool.get_info";
    }

    public static class Server
    {
        public const string Version = "server.version";
        public const string AddPeer = "server.add_peer";
        public const string Banner = "server.banner";
        public const string DonationAddress = "server.donation_address";
        public const string Features = "server.features";
        public const string PeersSubscribe = "server.peers.subscribe";
        public const string Ping = "server.ping";
    }

    public static class Daemon
    {
        public const string Passthrough = "daemon.passthrough";
    }

    public static class MasterNode
    {
        public const string AnnouncementBroadcast = "masternode.announce.broadcast";
        public const string Subscribe = "masternode.subscribe";
        public const string List = "masternode.list";
    }

    public static class ProTx
    {
        public const string Diff = "protx.diff";
        public const string Info = "protx.info";
    }
}
