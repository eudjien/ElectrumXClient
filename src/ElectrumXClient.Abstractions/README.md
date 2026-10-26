# NorthGomel.ElectrumXClient.Abstractions

Shared public contracts and protocol models for .NET clients that communicate with Electrum Cash Protocol servers.

This package contains the RPC client interfaces, typed request and response models, notification contracts, protocol
method names, and shared exceptions used by `NorthGomel.ElectrumXClient`.

Most applications should install [
`NorthGomel.ElectrumXClient`](https://www.nuget.org/packages/NorthGomel.ElectrumXClient/)
instead. Reference this package directly when implementing a compatible client or integration that needs the shared
protocol types without the default transport implementation.

For connection examples and the supported API surface, see the
[main project README](https://github.com/eudjien/ElectrumXClient#readme).
