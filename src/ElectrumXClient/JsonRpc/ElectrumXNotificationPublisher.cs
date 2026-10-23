using NorthGomel.ElectrumXClient.Abstractions;
using System;
using System.Diagnostics;

namespace NorthGomel.ElectrumXClient.JsonRpc;

internal static class ElectrumXNotificationPublisher
{
    public static void Publish<TEventArgs>(
        EventHandler<TEventArgs>? handlers,
        object source,
        TEventArgs eventArgs)
        where TEventArgs : ElectrumXNotificationEventArgs
    {
        if (handlers is null)
        {
            return;
        }

        foreach (var callback in handlers.GetInvocationList())
        {
            if (callback is not EventHandler<TEventArgs> handler)
            {
                continue;
            }

            try
            {
                handler(source, eventArgs);
            }
            catch (Exception exception)
            {
                Trace.TraceError(
                    "An ElectrumX notification handler for {0} threw an exception: {1}",
                    typeof(TEventArgs).Name,
                    exception);
            }
        }
    }
}
