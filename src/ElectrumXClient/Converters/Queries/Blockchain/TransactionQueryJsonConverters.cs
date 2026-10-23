using NorthGomel.ElectrumXClient.Abstractions.Models.Queries.Blockchain;
using System.Text.Json;

namespace NorthGomel.ElectrumXClient.Converters.Queries.Blockchain;

public sealed class UnspentOutputInfoQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXUnspentOutputInfoQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXUnspentOutputInfoQuery value, JsonSerializerOptions options)
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

public sealed class TransactionHeightQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXTransactionHeightQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXTransactionHeightQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        writer.WriteEndArray();
    }
}

public sealed class SubscribeToTransactionQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXSubscribeToTransactionQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXSubscribeToTransactionQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        writer.WriteEndArray();
    }
}

public sealed class UnsubscribeFromTransactionQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXUnsubscribeFromTransactionQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXUnsubscribeFromTransactionQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        writer.WriteEndArray();
    }
}

public sealed class GetDoubleSpendProofQueryJsonConverter : ElectrumXQueryJsonConverter<ElectrumXGetDoubleSpendProofQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXGetDoubleSpendProofQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.Hash);
        writer.WriteEndArray();
    }
}

public sealed class ListDoubleSpendProofTransactionsQueryJsonConverter
    : ElectrumXQueryJsonConverter<ElectrumXListDoubleSpendProofTransactionsQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXListDoubleSpendProofTransactionsQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteEndArray();
    }
}

public sealed class SubscribeToDoubleSpendProofQueryJsonConverter
    : ElectrumXQueryJsonConverter<ElectrumXSubscribeToDoubleSpendProofQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXSubscribeToDoubleSpendProofQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        writer.WriteEndArray();
    }
}

public sealed class UnsubscribeFromDoubleSpendProofQueryJsonConverter
    : ElectrumXQueryJsonConverter<ElectrumXUnsubscribeFromDoubleSpendProofQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXUnsubscribeFromDoubleSpendProofQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        writer.WriteEndArray();
    }
}

public sealed class ConfirmedTransactionBlockHashQueryJsonConverter
    : ElectrumXQueryJsonConverter<ElectrumXConfirmedTransactionBlockHashQuery>
{
    public override bool HandleNull => true;

    public override void Write(Utf8JsonWriter writer, ElectrumXConfirmedTransactionBlockHashQuery value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            throw new JsonException("An ElectrumX query cannot be null.");
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.TransactionHash);
        if (value.IncludeHeader)
        {
            writer.WriteBooleanValue(true);
        }

        writer.WriteEndArray();
    }
}
