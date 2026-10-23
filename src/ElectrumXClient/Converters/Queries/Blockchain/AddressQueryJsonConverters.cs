using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class AddressBalanceQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddressBalanceQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressBalanceQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        if (value.TokenFilter is not null)
        {
            writer.WriteStringValue(value.TokenFilter);
        }

        writer.WriteEndArray();
    }
}

public sealed class AddressHistoryQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddressHistoryQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressHistoryQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        if (value is not { FromHeight: 0, ToHeight: -1 })
        {
            writer.WriteNumberValue(value.FromHeight);
            writer.WriteNumberValue(value.ToHeight);
        }

        writer.WriteEndArray();
    }
}

public sealed class AddressMempoolQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddressMempoolQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressMempoolQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        writer.WriteEndArray();
    }
}

public sealed class AddressListUnspentQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddressListUnspentQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressListUnspentQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        if (value.TokenFilter is not null)
        {
            writer.WriteStringValue(value.TokenFilter);
        }

        writer.WriteEndArray();
    }
}

public sealed class AddressScriptHashQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddressScriptHashQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressScriptHashQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        writer.WriteEndArray();
    }
}

public sealed class AddressFirstUseQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddressFirstUseQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressFirstUseQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        writer.WriteEndArray();
    }
}

public sealed class AddressStatusQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXAddressStatusQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressStatusQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        writer.WriteEndArray();
    }
}

public sealed class SubscribeToAddressQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXSubscribeToAddressQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXSubscribeToAddressQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        writer.WriteEndArray();
    }
}

public sealed class UnsubscribeFromAddressQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXUnsubscribeFromAddressQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXUnsubscribeFromAddressQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Address);
        writer.WriteEndArray();
    }
}
