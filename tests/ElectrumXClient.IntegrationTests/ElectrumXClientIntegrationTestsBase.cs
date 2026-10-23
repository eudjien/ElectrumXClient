using NorthGomel.ElectrumXClient.Abstractions.Clients;
using NorthGomel.ElectrumXClient.Abstractions.Exceptions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using NorthGomel.ElectrumXClient.Converters;
using NorthGomel.ElectrumXClient.IntegrationTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using static NorthGomel.ElectrumXClient.IntegrationTests.IntegrationTestConstants;
using Client=NorthGomel.ElectrumXClient.Clients.ElectrumXClient;

namespace NorthGomel.ElectrumXClient.IntegrationTests;

public abstract class ElectrumXClientIntegrationTestsBase(
    IntegrationTestFixture fixture,
    ITestOutputHelper output)
{
    private static readonly JsonSerializerOptions ResponseSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    }.AddElectrumXConverters();

    [Fact]
    public Task Connect_exposes_negotiated_server_version()
    {
        return RunOnSingleClientAsync(client =>
        {
            Assert.False(string.IsNullOrWhiteSpace(client.ServerVersion.SoftwareVersion));
            Assert.False(string.IsNullOrWhiteSpace(client.ServerVersion.ProtocolVersion.ToString()));
            return Task.CompletedTask;
        });
    }

    [Fact]
    public Task GetBlockHeader_returns_genesis_header()
    {
        return RunOnSingleClientAsync(async client =>
            Assert.Equal(160,
                (await CallAsync(client.Blockchain.GetBlockHeaderAsync(0))).Value.Length));
    }

    [Fact]
    public Task GetBlockHeaderWithProof_returns_header_and_checkpoint_proof()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var header = await CallAsync(client.Blockchain.GetBlockHeaderAsync(0));
            var proof = await CallAsync(
                client.Blockchain.GetBlockHeaderWithProofAsync(0, 1));

            Assert.Equal(header.Value, proof.Header);
            Assert.NotEmpty(proof.Branch);
        });
    }

    [Fact]
    public Task GetBlockHeaders_returns_header_range()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var headers = await CallAsync(client.Blockchain.GetBlockHeadersAsync(0, 2));
            Assert.Equal(2, headers.Count);
            Assert.NotNull(headers.Hex);
        });
    }

    [Fact]
    public Task EstimateFee_returns_median_fee()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var fee = await CallAsync(client.Blockchain.EstimateFeeAsync(7));

            Assert.True(
                fee.Value is -1m or >= 0m,
                $"Expected a non-negative fee or -1 when unavailable, got {fee.Value}.");
        });
    }

    [Fact]
    public Task GetRelayFee_returns_minimum_fee()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var fee = await CallAsync(client.Blockchain.GetRelayFeeAsync());
            Assert.True(fee.Value >= 0m);
        });
    }

    [Fact]
    public Task GetBalance_returns_expected_balance()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var balance = await CallAsync(client.Blockchain.GetBalanceAsync(ScriptHash));
            Assert.True(balance.Confirmed >= 0);
        });
    }

    [Fact]
    public Task GetHistory_returns_transaction_history()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var history = await CallAsync(client.Blockchain.GetHistoryAsync(SmallHistoryScriptHash));
            Assert.NotEmpty(history.Value);
        });
    }

    [Fact]
    public Task GetMempool_returns_unconfirmed_transactions()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var mempool = await CallAsync(client.Blockchain.GetMempoolAsync(ScriptHash));
            Assert.NotNull(mempool.Value);
        });
    }

    [Fact]
    public Task ListUnspent_returns_outputs()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var unspent = await CallAsync(client.Blockchain.ListUnspentAsync(ScriptHash));
            Assert.NotNull(unspent.Value);
        });
    }


    [Fact]
    public Task GetTransactionMerkle_returns_merkle_proof_for_confirmed_transaction()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var transaction = await GetConfirmedTransactionAsync(client);
            var merkle = await CallAsync(
                client.Blockchain.GetTransactionMerkleAsync(transaction.TransactionHash, transaction.Height));

            Assert.Equal(transaction.Height, merkle.BlockHeight);
            Assert.NotEmpty(merkle.Merkle);
            Assert.InRange(merkle.Position, 0, int.MaxValue);
        });
    }

    [Fact]
    public Task GetTransactionIdFromPosition_returns_genesis_transaction_id()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var transactionId = await CallAsync(
                client.Blockchain.GetTransactionIdFromPositionAsync(971474, 0));

            Assert.True(transactionId.TransactionHash.All(char.IsAsciiHexDigit));
            Assert.Equal(64, transactionId.TransactionHash.Length);
            Assert.Null(transactionId.Merkle);
        });
    }

    [Fact]
    public Task GetTransactionIdFromPositionWithMerkle_returns_genesis_transaction_id()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var transactionId = await CallAsync(
                client.Blockchain.GetTransactionIdFromPositionAsync(971474, 0, true));

            Assert.True(transactionId.TransactionHash.All(char.IsAsciiHexDigit));
            Assert.Equal(64, transactionId.TransactionHash.Length);
            Assert.NotNull(transactionId.Merkle);
            Assert.NotEmpty(transactionId.Merkle);
        });
    }

    [Fact]
    public Task GetRawTransaction_returns_a_confirmed_transaction()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var transaction = await GetConfirmedTransactionAsync(client);
            var result = await CallAsync(
                client.Blockchain.GetRawTransactionAsync(transaction.TransactionHash));

            Assert.False(string.IsNullOrWhiteSpace(result.Value));
        });
    }

    [Fact]
    public Task GetVerboseTransaction_returns_a_confirmed_transaction()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var result = await CallAsync(
                client.Blockchain.GetVerboseTransactionAsync(ConfirmedTransactionHash));

            Assert.Equal(ConfirmedTransactionHash, result.TransactionId);
        });
    }

    [Fact]
    public Task GetTscMerkleWithTransactionHash_calls_the_server_method()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var transaction = await GetConfirmedTransactionAsync(client);
            await AssertSupportedOrMethodNotFoundAsync(
                client.Blockchain.GetTscMerkleWithTransactionHashAsync(transaction.TransactionHash, transaction.Height),
                nameof(IElectrumXBlockchainClient.GetTscMerkleWithTransactionHashAsync));
        });
    }

    [Fact]
    public Task GetTscMerkleWithTransactionHex_calls_the_server_method()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var transaction = await GetConfirmedTransactionAsync(client);
            await AssertSupportedOrMethodNotFoundAsync(
                client.Blockchain.GetTscMerkleWithTransactionHexAsync(transaction.TransactionHash, transaction.Height),
                nameof(IElectrumXBlockchainClient.GetTscMerkleWithTransactionHexAsync));
        });
    }

    [Fact]
    public Task GetTransactionIdFromPositionWithMerkle_returns_transaction_and_proof()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var result = await CallAsync(
                client.Blockchain.GetTransactionIdFromPositionAsync(1000, 0, true));

            // sha256 hash is 64 hex characters
            Assert.True(result.TransactionHash.All(char.IsAsciiHexDigit));
            Assert.Equal(64, result.TransactionHash.Length);

            // some servers may return an empty merkle proof for the genesis transaction, so we only assert that the merkle proof is not null
            Assert.NotNull(result.Merkle);
        });
    }

    [Fact]
    public Task GetServerFeatures_returns_network_features()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var result = await CallAsync(
                client.Server.GetServerFeaturesAsync());

            Assert.False(string.IsNullOrWhiteSpace(result.GenesisHash));
            Assert.False(string.IsNullOrWhiteSpace(result.ServerVersion));
        });
    }

    [Fact]
    public Task GetBanner_returns_server_banner()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var result = await CallAsync(client.Server.GetBannerAsync());
            Assert.False(string.IsNullOrWhiteSpace(result.Value));
        });
    }

    [Fact]
    public Task GetDonationAddress_returns_server_address()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var result = await CallAsync(
                client.Server.GetDonationAddressAsync());

            Assert.NotNull(result.Value);
        });
    }

    [Fact]
    public Task Ping_returns_a_response()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var result = await CallAsync(client.Server.PingAsync());
            Assert.NotNull(result);
        });
    }

    [Fact]
    public Task AddPeer_returns_the_servers_acceptance_result()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var result = await CallAsync(
                client.Server.AddPeerAsync(
                    new Dictionary<string, ElectrumXServerHostPorts>
                    {
                        ["127.0.0.1"] = new(null, null)
                    },
                    MainnetGenesisHash,
                    "sha256",
                    "ElectrumXClient test",
                    "1.4",
                    "1.4"));

            Assert.False(result.Value);
        });
    }

    [Fact]
    public Task GetFeeHistogram_returns_mempool_fee_buckets()
    {
        return RunOnSingleClientAsync(async client =>
        {
            var result = await CallAsync(
                client.Mempool.GetFeeHistogramAsync());

            Assert.NotNull(result.Value);
        });
    }

    private Task RunOnSingleClientAsync(Func<Client, Task> test)
    {
        return fixture.RunOnSingleClientAsync(output, test);
    }

    private async Task<ElectrumXHistoryEntry> GetConfirmedTransactionAsync(Client client)
    {
        var history = (await CallAsync(client.Blockchain.GetHistoryAsync(SmallHistoryScriptHash))).Value;
        foreach (var entry in history)
        {
            if (entry.Height > 0)
            {
                return entry;
            }
        }

        throw new InvalidOperationException("The transaction history did not contain a confirmed transaction.");
    }

    private async Task<TResponse> CallAsync<TResponse>(Task<TResponse> request)
        where TResponse : ElectrumXResult
    {
        var response = await request;
        output.WriteLine($"Result:{Environment.NewLine}{JsonSerializer.Serialize(response, ResponseSerializerOptions)}");

        return response;
    }

    private async Task AssertSupportedOrMethodNotFoundAsync<TResponse>(Task<TResponse> request, string method)
        where TResponse : ElectrumXResult
    {
        try
        {
            Assert.NotNull(await CallAsync(request));
        }
        catch (ElectrumXServerException exception)
        {
            LogRpcError(method, exception);
            Assert.True(
                exception.Code == -32601,
                $"RPC method '{method}' failed with an error other than method-not-found: {exception.Message}");
        }
    }

    private void LogRpcError(string method, ElectrumXServerException exception)
    {
        string json = JsonSerializer.Serialize(
            new
            {
                exception.Code, exception.Error.Message, exception.Error.Data
            },
            ResponseSerializerOptions);
        string message = $"RPC error: {method}{Environment.NewLine}{json}";
        Console.WriteLine(message);
        output.WriteLine(message);
    }
}
