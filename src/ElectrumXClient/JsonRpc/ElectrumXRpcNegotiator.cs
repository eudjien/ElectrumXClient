using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Exceptions;
using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Server;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.JsonRpc;

internal static class ElectrumXRpcNegotiator
{
    public static async Task<ElectrumXServerVersion> NegotiateAsync(IElectrumXRpcClient rpcClient, CancellationToken cancellationToken)
    {
        var options = rpcClient.ClientOptions;

        var versionResponse = await rpcClient.CallAsync<ElectrumXServerVersion, ElectrumXServerVersionQuery>(
                new JsonRpcRequest<ElectrumXServerVersionQuery>(
                    ElectrumXJsonMethodNames.Server.Version,
                    new ElectrumXServerVersionQuery(
                        options.ClientName,
                        options.ProtocolVersions.Minimum.ToString(),
                        options.ProtocolVersions.Maximum.ToString())),
                cancellationToken)
            .ConfigureAwait(false);

        var version = versionResponse.Result ?? throw new ElectrumXProtocolException("The ElectrumX result response did not contain data.");

        if (string.IsNullOrWhiteSpace(version.SoftwareVersion))
        {
            throw new ElectrumXProtocolException(
                "The server.version response must contain the software and protocol versions.");
        }

        if (version.ProtocolVersion < options.ProtocolVersions.Minimum ||
            version.ProtocolVersion > options.ProtocolVersions.Maximum)
        {
            throw new ElectrumXProtocolException(
                $"The negotiated protocol version {version.ProtocolVersion} is outside the client-supported range " +
                $"{options.ProtocolVersions.Minimum} to {options.ProtocolVersions.Maximum}.");
        }

        return version;
    }
}
