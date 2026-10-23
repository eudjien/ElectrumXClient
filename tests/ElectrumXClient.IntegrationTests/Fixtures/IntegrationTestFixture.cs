using System;
using System.Threading.Tasks;
using Xunit;

namespace NorthGomel.ElectrumXClient.IntegrationTests.Fixtures;

public abstract class IntegrationTestFixture : IAsyncLifetime
{
    private readonly IntegrationTestServer _server;
    private Clients.ElectrumXClient? _client;

    private protected IntegrationTestFixture(IntegrationTestServer server)
    {
        _server = server;
    }

    public async ValueTask InitializeAsync()
    {
        _client = await Clients.ElectrumXClient.ConnectAsync(_server.RpcClientFactory());
    }

    public async ValueTask DisposeAsync()
    {
        if (_client is not null)
        {
            await _client.DisposeAsync();
            _client = null;
        }
    }

    internal async Task RunOnSingleClientAsync(ITestOutputHelper output, Func<Clients.ElectrumXClient, Task> test)
    {
        var client = _client ?? throw new InvalidOperationException("The integration test client is not initialized.");
        output.WriteLine(
            $"Testing System.Text.Json against {_server.Transport} ({_server.Host}:{_server.Port})");

        try
        {
            await test(client);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Integration test failed against {_server.Transport} ({_server.Host}:{_server.Port}) " +
                "using System.Text.Json.",
                exception);
        }
    }
}
