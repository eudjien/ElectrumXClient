using NorthGomel.ElectrumXClient.IntegrationTests.Fixtures;
using Xunit;

namespace NorthGomel.ElectrumXClient.IntegrationTests;

public sealed class WebSocketSystemTextJsonElectrumXClientIntegrationTests(
    WebSocketSystemTextJsonIntegrationTestFixture fixture,
    ITestOutputHelper output)
    : ElectrumXClientIntegrationTestsBase(fixture, output),
        IClassFixture<WebSocketSystemTextJsonIntegrationTestFixture>;
