using NorthGomel.ElectrumXClient.Abstractions;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace NorthGomel.ElectrumXClient.JsonRpc.Clients;

/// <summary>Configures TLS authentication options using the ElectrumX RPC options.</summary>
/// <param name="authenticationOptions">The TLS authentication options to configure.</param>
/// <param name="rpcClientOptions">The RPC options supplied to the client constructor.</param>
public delegate void ElectrumXSslRpcClientAuthConfigurator(
    SslClientAuthenticationOptions authenticationOptions,
    IElectrumXRpcClientOptions rpcClientOptions);

/// <summary>Configures TLS authentication options for an ElectrumX SSL client.</summary>
/// <param name="authenticationOptions">The TLS authentication options to configure.</param>
public delegate void ElectrumXSslRpcClientAuthOptionsConfigurator(
    SslClientAuthenticationOptions authenticationOptions);

internal static class ElectrumXSslRpcClientDefaults
{
    internal static SslClientAuthenticationOptions CreateAuthenticationOptions(string? targetHost = null)
    {
        return new SslClientAuthenticationOptions
        {
            TargetHost = targetHost,
            EnabledSslProtocols = SslProtocols.None,
            CertificateRevocationCheckMode = X509RevocationMode.NoCheck,
            RemoteCertificateValidationCallback = (_, _, _, _) => true
        };
    }
}

/// <summary>JSON-RPC transport that communicates over a TLS-encrypted TCP connection.</summary>
public sealed class ElectrumXTlsTcpRpcClient : ElectrumXTcpRpcClientBase
{
    private readonly SslClientAuthenticationOptions _authenticationOptions;

    /// <summary>Creates a TLS-encrypted TCP JSON-RPC client with TLS-specific configuration.</summary>
    /// <param name="clientOptions">The ElectrumX RPC options.</param>
    /// <param name="configureAuthenticationOptions">Callback to configure TLS authentication settings.</param>
    public ElectrumXTlsTcpRpcClient(
        IElectrumXRpcClientOptions clientOptions,
        ElectrumXSslRpcClientAuthOptionsConfigurator configureAuthenticationOptions)
        : this(clientOptions, CreateOptionsConfigurator(configureAuthenticationOptions))
    {
    }

    /// <summary>Creates a TLS-encrypted TCP JSON-RPC client with TLS and RPC configuration.</summary>
    /// <param name="clientOptions">The ElectrumX RPC options.</param>
    /// <param name="configureAuthenticationOptions">
    ///     Callback to configure TLS authentication settings using the RPC options supplied to this constructor.
    /// </param>
    public ElectrumXTlsTcpRpcClient(
        IElectrumXRpcClientOptions clientOptions,
        ElectrumXSslRpcClientAuthConfigurator? configureAuthenticationOptions = null)
        : base(clientOptions)
    {
        var authenticationOptions = ElectrumXSslRpcClientDefaults.CreateAuthenticationOptions(clientOptions.Host);
        configureAuthenticationOptions?.Invoke(authenticationOptions, clientOptions);

        _authenticationOptions = authenticationOptions;
    }

    /// <summary>The TLS authentication settings used by this client.</summary>
    public SslClientAuthenticationOptions AuthenticationOptions => _authenticationOptions;

    private static ElectrumXSslRpcClientAuthConfigurator CreateOptionsConfigurator(
        ElectrumXSslRpcClientAuthOptionsConfigurator configureAuthenticationOptions)
    {
        Abstractions.ArgumentGuard.ThrowIfNull(configureAuthenticationOptions, nameof(configureAuthenticationOptions));
        return (authenticationOptions, _) => configureAuthenticationOptions(authenticationOptions);
    }

    /// <inheritdoc />
    protected override async ValueTask<Stream> CreateConnectedStreamAsync(
        NetworkStream networkStream,
        CancellationToken cancellationToken)
    {
        var sslStream = new SslStream(networkStream);
        await sslStream.AuthenticateAsClientAsync(
            AuthenticationOptions,
            cancellationToken).ConfigureAwait(false);
        return sslStream;
    }
}
