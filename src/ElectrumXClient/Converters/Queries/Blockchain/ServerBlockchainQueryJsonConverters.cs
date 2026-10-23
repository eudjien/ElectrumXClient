using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class HeaderByHashQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXHeaderByHashQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXHeaderByHashQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.BlockHash);
        writer.WriteEndArray();
    }
}

public sealed class HeaderByHeightQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXHeaderByHeightQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXHeaderByHeightQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.Height);
        writer.WriteEndArray();
    }
}

public sealed class GetHeaderTipQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXGetHeaderTipQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXGetHeaderTipQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteEndArray();
    }
}

public sealed class UnsubscribeFromHeadersQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXUnsubscribeFromHeadersQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXUnsubscribeFromHeadersQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteEndArray();
    }
}

public sealed class ScriptHashStatusQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXScriptHashStatusQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXScriptHashStatusQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.ScriptHash);
        writer.WriteEndArray();
    }
}

public sealed class ScriptHashFirstUseQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXScriptHashFirstUseQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXScriptHashFirstUseQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.ScriptHash);
        writer.WriteEndArray();
    }
}

public sealed class RpaHistoryQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXRpaHistoryQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXRpaHistoryQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.RpaPrefix);
        writer.WriteNumberValue(value.FromHeight);
        writer.WriteNumberValue(value.ToHeight);
        writer.WriteEndArray();
    }
}

public sealed class RpaMempoolQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXRpaMempoolQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXRpaMempoolQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.RpaPrefix);
        writer.WriteEndArray();
    }
}

public sealed class ReusableHistoryQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXReusableHistoryQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXReusableHistoryQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.FromHeight);
        writer.WriteNumberValue(value.Count);
        writer.WriteStringValue(value.RpaPrefix);
        writer.WriteEndArray();
    }
}

public sealed class ReusableMempoolQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXReusableMempoolQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXReusableMempoolQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.RpaPrefix);
        writer.WriteEndArray();
    }
}

public sealed class BroadcastPackageQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXBroadcastPackageQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXBroadcastPackageQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStartArray();
        foreach (string transaction in value.RawTransactions)
        {
            writer.WriteStringValue(transaction);
        }

        writer.WriteEndArray();
        writer.WriteBooleanValue(value.Verbose);
        writer.WriteEndArray();
    }
}

public sealed class LegacyBlockHeaderQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXLegacyBlockHeaderQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXLegacyBlockHeaderQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.Height);
        writer.WriteEndArray();
    }
}

public sealed class BlockHeaderChunkQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXBlockHeaderChunkQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXBlockHeaderChunkQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteNumberValue(value.Index);
        writer.WriteEndArray();
    }
}

public sealed class NumBlocksSubscribeQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXNumBlocksSubscribeQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXNumBlocksSubscribeQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteEndArray();
    }
}

public sealed class UtxoAddressQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXUtxoAddressQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXUtxoAddressQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        writer.WriteNumberValue(value.OutputIndex);
        writer.WriteEndArray();
    }
}
