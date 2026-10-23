using NorthGomel.ElectrumXClient.IntegrationTests.Fixtures;
using Xunit;

namespace NorthGomel.ElectrumXClient.IntegrationTests;

public sealed class TcpSystemTextJsonElectrumXClientIntegrationTests(
    TcpSystemTextJsonIntegrationTestFixture fixture,
    ITestOutputHelper output)
    : ElectrumXClientIntegrationTestsBase(fixture, output),
        IClassFixture<TcpSystemTextJsonIntegrationTestFixture>;
