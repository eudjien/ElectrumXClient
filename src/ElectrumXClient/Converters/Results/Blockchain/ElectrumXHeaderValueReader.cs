using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models;
using System.Globalization;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

internal static class ElectrumXHeaderValueReader
{
    public static (long? Height, string? Hex, bool HasRawHeight) ReadFields(ElectrumXJsonValue value)
    {
        if (value is not ElectrumXJsonValue.ObjectValue objectValue)
        {
            return (null, null, false);
        }

        long? height = null;
        string? hex = null;
        bool hasRawHeight = false;

        foreach ((string name, var propertyValue) in objectValue.Properties)
        {
            switch (name)
            {
                case ElectrumXJsonPropertyNames.BlockHeaderSubscriptionResult.Height:
                    hasRawHeight = true;
                    goto case ElectrumXJsonPropertyNames.DeserializedBlockHeader.BlockHeight;
                case ElectrumXJsonPropertyNames.DeserializedBlockHeader.BlockHeight:
                    height = propertyValue is ElectrumXJsonValue.NumberValue numberValue &&
                             long.TryParse(numberValue.RawJson, NumberStyles.Integer, CultureInfo.InvariantCulture, out long parsedHeight)
                        ? parsedHeight
                        : throw new JsonException("The block-header height must be a 64-bit integer.");
                    break;
                case ElectrumXJsonPropertyNames.BlockHeaderSubscriptionResult.Hex:
                    hex = propertyValue is ElectrumXJsonValue.StringValue stringValue
                        ? stringValue.Value
                        : throw new JsonException("The block-header hex must be a string.");
                    break;
            }
        }

        return (height, hex, hasRawHeight);
    }
}
