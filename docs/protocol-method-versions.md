# Electrum Cash protocol method versions

This reference covers the JSON-RPC methods exposed by this client. It is based on the
[Electrum Cash Protocol method reference](https://electrum-cash-protocol.readthedocs.io/en/latest/protocol-methods.html),
[protocol changes](https://electrum-cash-protocol.readthedocs.io/en/latest/protocol-changes.html), and
[removed methods](https://electrum-cash-protocol.readthedocs.io/en/latest/protocol-removed.html).

The availability column shows the protocol-version range in which a method is available. A `+` means no removal
version is documented. Where a method was removed and later reintroduced, each availability period is listed
separately. Server implementation and advertised capabilities can impose additional requirements. A deprecated
method is not necessarily removed; this client does not reject a call solely because the method is deprecated.

## Blockchain

| JSON-RPC method | Available protocol versions | Deprecated / removed / changes |
| --- | --- | --- |
| `blockchain.address.get_balance` | 1.0-1.2; 1.4.3+ | Reintroduced in 1.4.3. The earlier address API was deprecated in 1.2 and removed in 1.3. Optional `token_filter` added in 1.5.0. |
| `blockchain.address.get_first_use` | 1.5.2+ | No deprecation or removal documented. |
| `blockchain.address.get_history` | 1.0-1.2; 1.4.3+ | Reintroduced in 1.4.3 after the earlier address API was deprecated in 1.2 and removed in 1.3. Pagination added in 1.5.1; canonical mempool ordering specified in 1.6.0. |
| `blockchain.address.get_mempool` | 1.0-1.2; 1.4.3+ | Reintroduced in 1.4.3 after the earlier address API was deprecated in 1.2 and removed in 1.3. Canonical ordering specified in 1.6.0. |
| `blockchain.address.get_scripthash` | 1.4.3+ | No deprecation or removal documented. |
| `blockchain.address.get_status` | 1.6.0+ | Fulcrum 2.1.1 or later only. |
| `blockchain.address.listunspent` | 1.0-1.2; 1.4.3+ | Reintroduced in 1.4.3 after the earlier address API was deprecated in 1.2 and removed in 1.3. Optional `token_filter` and optional `token_data` result field added in 1.5.0. |
| `blockchain.address.subscribe` | 1.0-1.2; 1.4.3+ | Reintroduced in 1.4.3 after the earlier address API was deprecated in 1.2 and removed in 1.3. |
| `blockchain.address.unsubscribe` | 1.4.3+ | No deprecation or removal documented. |
| `blockchain.block.get_chunk` | 1.0-1.3 | Deprecated in 1.2; removed in 1.4. Use `blockchain.block.headers`. |
| `blockchain.block.get_header` | 1.0-1.3 | Deprecated in 1.3; removed in 1.4. Use `blockchain.block.header`. |
| `blockchain.block.header` | 1.3+ | Optional `cp_height` checkpoint-proof argument added in 1.4. |
| `blockchain.block.headers` | 1.2+ | Optional `cp_height` checkpoint-proof argument added in 1.4. In 1.6.0, the response changed from a concatenated `hex` value to a `headers` array. |
| `blockchain.estimatefee` | 1.0+ | Optional daemon-specific `mode` argument added in 1.6.0 (BTC/LTC only). The client API remarks report ElectrumX deprecation from 1.4.2; the current Electrum Cash protocol change log does not list this method as removed. |
| `blockchain.header.get` | 1.5.2+ | Accepts a block hash; the protocol also permits a numeric height. No deprecation or removal documented. |
| `blockchain.headers.get_tip` | 1.5.0+ | No deprecation or removal documented. |
| `blockchain.headers.subscribe` | 1.0+ | Optional `raw` argument existed in 1.2-1.3 only. Deserialized-header responses were deprecated in 1.2 and removed in 1.4; raw headers are used from 1.4 onward. |
| `blockchain.headers.unsubscribe` | 1.5.0+ | No deprecation or removal documented. |
| `blockchain.numblocks.subscribe` | 1.0 only | Deprecated in 1.0; removed in 1.1. |
| `blockchain.relayfee` | 1.0+ | The client API remarks report ElectrumX deprecation from 1.4.2; the protocol change log marks it deprecated in 1.6.0 and recommends `mempool.get_info`. No removal is documented. |
| `blockchain.reusable.get_history` | 1.5.3+ | Deprecated compatibility method for legacy clients; use `blockchain.rpa.get_history`. No removal is documented. |
| `blockchain.reusable.get_mempool` | 1.5.3+ | Deprecated compatibility method for legacy clients; use `blockchain.rpa.get_mempool`. No removal is documented. |
| `blockchain.rpa.get_history` | 1.5.3+ | No deprecation or removal documented. Requires a server with RPA indexing support. |
| `blockchain.rpa.get_mempool` | 1.5.3+ | No deprecation or removal documented. Requires a server with RPA indexing support. |
| `blockchain.scripthash.get_balance` | 1.1+ | Optional CashToken `token_filter` added in 1.5.0. |
| `blockchain.scripthash.get_first_use` | 1.5.2+ | No deprecation or removal documented. |
| `blockchain.scripthash.get_history` | 1.1+ | Pagination added in 1.5.1; canonical mempool ordering specified in 1.6.0. |
| `blockchain.scripthash.get_mempool` | 1.1+ | Canonical mempool ordering specified in 1.6.0. |
| `blockchain.scripthash.get_status` | 1.6.0+ | Fulcrum 2.1.1 or later only. |
| `blockchain.scripthash.listunspent` | 1.1+ | Optional CashToken `token_filter` and optional `token_data` result field added in 1.5.0. |
| `blockchain.scripthash.subscribe` | 1.1+ | No deprecation or removal documented. |
| `blockchain.scripthash.unsubscribe` | 1.4.2+ | The method reference says 1.4.2. The change log labels it 1.4.1 but misspells the method as `scipthash.unsubscribe`; this appears to be a typo. |
| `blockchain.transaction.broadcast` | 1.0+ | JSON-RPC errors are returned as errors rather than successful results from 1.1. |
| `blockchain.transaction.broadcast_package` | 1.6.0+ | BTC only; server support is implementation-dependent and may be advertised by `server.features`. |
| `blockchain.transaction.dsproof.get` | 1.4.5+ | No deprecation or removal documented; server/daemon support is implementation-dependent. |
| `blockchain.transaction.dsproof.list` | 1.4.5+ | No deprecation or removal documented; server/daemon support is implementation-dependent. |
| `blockchain.transaction.dsproof.subscribe` | 1.4.5+ | No deprecation or removal documented; server/daemon support is implementation-dependent. |
| `blockchain.transaction.dsproof.unsubscribe` | 1.4.5+ | No deprecation or removal documented; server/daemon support is implementation-dependent. |
| `blockchain.transaction.get` | 1.0+ | The ignored `height` argument was removed in 1.1; optional `verbose` argument added in 1.2. |
| `blockchain.transaction.get_confirmed_blockhash` | 1.5.2+ | No deprecation or removal documented. |
| `blockchain.transaction.get_height` | 1.4.5+ | No deprecation or removal documented. |
| `blockchain.transaction.get_merkle` | 1.0+ | The `height` argument became optional in 1.4.5. |
| `blockchain.transaction.get_tsc_merkle` | Not specified | TSC extension; depends on server implementation. This client supports branch proofs, not composite proofs. |
| `blockchain.transaction.id_from_pos` | 1.4.0+ | No deprecation or removal documented. |
| `blockchain.transaction.subscribe` | 1.4.5+ | No deprecation or removal documented. |
| `blockchain.transaction.unsubscribe` | 1.4.5+ | No deprecation or removal documented. |
| `blockchain.utxo.get_address` | 1.0 only | Optional in 1.0; deprecated and removed in 1.1. |
| `blockchain.utxo.get_info` | 1.4.4+ | Optional `token_data` result field added in 1.5.0. |

## Server

| JSON-RPC method | Available protocol versions | Deprecated / removed / changes |
| --- | --- | --- |
| `daemon.passthrough` | 1.5.2+ | Disabled by default on most servers because it forwards RPC calls to the backing daemon. |
| `server.add_peer` | 1.1+ | Intended for server software, not ordinary wallet clients. |
| `server.banner` | 1.0+ | No deprecation or removal documented. |
| `server.donation_address` | 1.0+ | No deprecation or removal documented. |
| `server.features` | 1.1+ | `hosts` became recommended rather than required in 1.4.2; optional `dsproof`, `cashtokens`, `rpa`, and `broadcast_package` capabilities were added in 1.4.5, 1.5.0, 1.5.3, and 1.6.0 respectively. |
| `server.peers.subscribe` | 1.0+ | Despite its name, the protocol defines this as a one-time request without notifications. |
| `server.ping` | 1.2+ | Use this instead of `server.version` as a ping mechanism. |
| `server.version` | 1.0+ | Negotiation semantics changed in 1.1; from 1.2 it should not be used as a ping; only the first request is accepted from 1.4. |

## ProTx

These methods depend on servers supporting Dash DIP3.

| JSON-RPC method | Available protocol versions | Deprecated / removed / changes |
| --- | --- | --- |
| `protx.diff` | Not specified | Dash DIP3 server extension; no Electrum Cash protocol availability version is documented. |
| `protx.info` | Not specified | Dash DIP3 server extension; no Electrum Cash protocol availability version is documented. |

## Mempool

| JSON-RPC method | Available protocol versions | Deprecated / removed / changes |
| --- | --- | --- |
| `mempool.get_fee_histogram` | 1.2+ | The client API remarks report ElectrumX deprecation from 1.4.2. The protocol reference does not list a removal version. |
| `mempool.get_info` | 1.6.0+ | Provides more general mempool relay and policy information; replaces `blockchain.relayfee` for that purpose. |

## Masternode

These methods depend on compatible Dash-derived ElectrumX servers.

| JSON-RPC method | Available protocol versions | Deprecated / removed / changes |
| --- | --- | --- |
| `masternode.announce.broadcast` | Not specified | Dash-derived ElectrumX server extension. |
| `masternode.list` | Not specified | Compatible Dash-derived server extension. |
| `masternode.subscribe` | Not specified | Compatible Dash-derived server extension. |

## Notes

- The address methods `get_balance`, `get_history`, `get_mempool`, `listunspent`, and `subscribe` were available in
  1.0–1.2, deprecated in 1.2, removed in 1.3, and reintroduced in 1.4.3.
- A protocol version is a negotiated range, not a guarantee that every optional, coin-specific, or server-specific
  method is implemented. Check server capability documentation and responses where applicable.
- The client may proactively reject methods that the protocol explicitly removed for the negotiated version. A
  deprecated-but-not-removed method remains callable.
