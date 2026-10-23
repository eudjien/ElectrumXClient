using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.JsonRpc.Clients;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ElectrumClient=NorthGomel.ElectrumXClient.Clients.ElectrumXClient;
using ElectrumXRpcClientOptions=NorthGomel.ElectrumXClient.JsonRpc.ElectrumXRpcClientOptions;

namespace NorthGomel.ElectrumXClient.EventMonitor;

internal static partial class Program
{
    private const string ServerHost = "127.0.0.1";
    private const int ServerPort = 50002;

    // https://www.blockchain.com/explorer/addresses/btc/1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa
    private const string BitcoinAddressToSubscribe = "bc1qns9f7yfx3ry9lj6yz7c9er0vwa0ye2eklpzqfw";
    private static readonly string ScriptHashToSubscribe = ElectrumXScriptHash.FromBitcoinAddress(BitcoinAddressToSubscribe);

    private static readonly Lock ConsoleLock = new();

    /// <summary>Connects, subscribes to available notification streams, and prints updates until cancellation.</summary>
    private static async Task<int> Main()
    {
        var shutdown = new CancellationTokenSource();
        using var cancelHandler = new ShutdownHandler(shutdown);

        Console.CancelKeyPress += cancelHandler.Handle;

        try
        {
            var options = new ElectrumXRpcClientOptions
            {
                Host = ServerHost, Port = ServerPort
            };
            var rpcClient = new ElectrumXTlsTcpRpcClient(options);

            await using var client = await ElectrumClient.ConnectAsync(
                rpcClient,
                cancellationToken: shutdown.Token).ConfigureAwait(false);

            bool isScriptHashSubscribed = false;

            try
            {
                client.Blockchain.BlockHeaderReceived += OnBlockHeaderReceived;
                client.Blockchain.ScriptHashStatusChanged += OnScriptHashStatusChanged;

                WriteLine($"Connected to {ServerHost}:{ServerPort}. Server: {client.ServerVersion.SoftwareVersion}");

                await TrySubscribeAsync(
                    "block headers",
                    client.Blockchain.SubscribeToHeadersAsync(shutdown.Token),
                    chainTip => WriteLine($"Current chain tip: height={chainTip.Height}, hex={chainTip.Hex}"),
                    shutdown.Token).ConfigureAwait(false);

                isScriptHashSubscribed = await TrySubscribeAsync(
                    $"script hash {ScriptHashToSubscribe}, address {BitcoinAddressToSubscribe}",
                    client.Blockchain.SubscribeToScriptHashAsync(ScriptHashToSubscribe, shutdown.Token),
                    status => WriteLine(
                        $"Subscribed to script hash {ScriptHashToSubscribe}; current status={status.Value ?? "(none)"}"),
                    shutdown.Token).ConfigureAwait(false);


                WriteLine("Listening for events. Press Ctrl+C to stop.");
                await Task.Delay(Timeout.InfiniteTimeSpan, shutdown.Token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (shutdown.IsCancellationRequested)
            {
                WriteLine("Stopping event monitor.");
            }
            finally
            {
                if (isScriptHashSubscribed)
                {
                    try
                    {
                        var result = await client.Blockchain.UnsubscribeFromScriptHashAsync(
                            ScriptHashToSubscribe,
                            CancellationToken.None).ConfigureAwait(false);
                        WriteLine($"Unsubscribed from script hash {ScriptHashToSubscribe}: {result.Value}");
                    }
                    catch (Exception exception)
                    {
                        WriteLine($"Failed to unsubscribe from script hash {ScriptHashToSubscribe}: {exception.Message}");
                    }
                }
            }

            return 0;
        }
        catch (OperationCanceledException) when (shutdown.IsCancellationRequested)
        {
            return 0;
        }
        catch (Exception exception)
        {
            WriteLine($"Event monitor failed: {exception}");
            return 1;
        }
        finally
        {
            Console.CancelKeyPress -= cancelHandler.Handle;
            cancelHandler.CompleteShutdown();
            shutdown.Dispose();
        }
    }

    /// <summary>Runs one subscription, reports its result, and allows other subscriptions after a failure.</summary>
    private static async Task<bool> TrySubscribeAsync<TResult>(
        string subscriptionName,
        Task<TResult> subscription,
        Action<TResult> onSuccess,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await subscription.ConfigureAwait(false);
            onSuccess(result);

            return true;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            WriteLine($"Failed to subscribe to {subscriptionName}: {exception.Message}");

            return false;
        }
    }

    /// <summary>Prints a received block-header notification.</summary>
    private static void OnBlockHeaderReceived(object? sender, ElectrumXBlockHeaderEventArgs args)
    {
        WriteLine($"Block header: height={args.Notification.Height}, hex={args.Notification.Hex}");
    }

    /// <summary>Prints a received script-hash status notification.</summary>
    private static void OnScriptHashStatusChanged(object? sender, ElectrumXScriptHashStatusEventArgs args)
    {
        WriteLine($"Script hash status: hash={args.Notification.ScriptHash}, status={args.Notification.Status ?? "(none)"}");
    }

    /// <summary>Writes a timestamped line while preventing concurrent event handlers from interleaving output.</summary>
    private static void WriteLine(string message)
    {
        lock (ConsoleLock)
        {
            Console.Write('[');
            Console.Write(DateTimeOffset.Now.ToString("O", CultureInfo.InvariantCulture));
            Console.Write(']');
            Console.Write(' ');
            Console.WriteLine(message);
        }
    }

    private sealed partial class ShutdownHandler : IDisposable
    {
        private const uint CtrlCloseEvent = 2;
        private const uint CtrlLogoffEvent = 5;
        private const uint CtrlShutdownEvent = 6;
        private static readonly TimeSpan CloseEventCleanupTimeout = TimeSpan.FromSeconds(4);

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ConsoleControlHandler? _consoleControlHandler;
        private readonly ManualResetEventSlim _shutdownCompleted = new(false);

        internal ShutdownHandler(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;

            if (!OperatingSystem.IsWindows())
            {
                return;
            }

            _consoleControlHandler = HandleConsoleControl;

            if (!SetConsoleCtrlHandler(_consoleControlHandler, true))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to register the console close handler.");
            }
        }

        public void Dispose()
        {
            if (_consoleControlHandler is not null && !SetConsoleCtrlHandler(_consoleControlHandler, false))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to unregister the console close handler.");
            }

            _shutdownCompleted.Dispose();
        }

        internal void Handle(object? sender, ConsoleCancelEventArgs eventArgs)
        {
            eventArgs.Cancel = true;
            _cancellationTokenSource.Cancel();
        }

        internal void CompleteShutdown()
        {
            _shutdownCompleted.Set();
        }

        private bool HandleConsoleControl(uint controlType)
        {
            if (controlType is not (CtrlCloseEvent or CtrlLogoffEvent or CtrlShutdownEvent))
            {
                return false;
            }

            _cancellationTokenSource.Cancel();
            _shutdownCompleted.Wait(CloseEventCleanupTimeout);

            return true;
        }

        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool SetConsoleCtrlHandler(ConsoleControlHandler handler, [MarshalAs(UnmanagedType.Bool)] bool add);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private delegate bool ConsoleControlHandler(uint controlType);
    }
}
