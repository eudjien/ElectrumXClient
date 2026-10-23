using NorthGomel.ElectrumXClient.IntegrationTests.Fixtures;
using Xunit;

namespace NorthGomel.ElectrumXClient.IntegrationTests;

public sealed class SslSystemTextJsonElectrumXClientIntegrationTests(
    SslSystemTextJsonIntegrationTestFixture fixture,
    ITestOutputHelper output)
    : ElectrumXClientIntegrationTestsBase(fixture, output),
        IClassFixture<SslSystemTextJsonIntegrationTestFixture>;
