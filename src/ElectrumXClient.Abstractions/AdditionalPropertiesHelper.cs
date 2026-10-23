using NorthGomel.ElectrumXClient.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorthGomel.ElectrumXClient.Abstractions;

internal static class AdditionalPropertiesHelper
{
    internal static IReadOnlyDictionary<string, ElectrumXJsonValue> Copy(
        IReadOnlyDictionary<string, ElectrumXJsonValue>? additionalProperties,
        Func<string, bool> isKnownProperty)
    {
        var additionalPropertiesCopy = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);

        if (additionalProperties is null)
        {
            return new ReadOnlyDictionary<string, ElectrumXJsonValue>(additionalPropertiesCopy);
        }

        foreach ((string name, var value) in additionalProperties)
        {
            if (name is null)
            {
                throw new ArgumentException("Additional property names cannot be null.", nameof(additionalProperties));
            }

            if (isKnownProperty(name))
            {
                throw new ArgumentException(
                    $"'{name}' is a standard property and cannot be an additional property.",
                    nameof(additionalProperties));
            }

            additionalPropertiesCopy.Add(
                name,
                value ?? throw new ArgumentException(
                    "Additional property values cannot be null; use ElectrumXJsonValue.NullValue.Instance.",
                    nameof(additionalProperties)));
        }

        return new ReadOnlyDictionary<string, ElectrumXJsonValue>(additionalPropertiesCopy);
    }
}
