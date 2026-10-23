using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Blockchain;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Mempool;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Blockchain;

public sealed class AddressScriptHashResultJsonConverter : JsonConverter<ElectrumXAddressScriptHash>
{
    public override ElectrumXAddressScriptHash Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType == JsonTokenType.String
            ? new ElectrumXAddressScriptHash(reader.GetString() ?? throw new JsonException("The script hash cannot be null."))
            : throw new JsonException("The script hash result must be a string.");
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressScriptHash value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ScriptHash);
    }
}

public sealed class AddressStatusResultJsonConverter : JsonConverter<ElectrumXAddressStatus>
{
    public override bool HandleNull => true;

    public override ElectrumXAddressStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new ElectrumXAddressStatus(reader.GetString()),
            JsonTokenType.Null => new ElectrumXAddressStatus(null),
            _ => throw new JsonException("The result must be a string or null.")
        };
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressStatus value, JsonSerializerOptions options)
    {
        if (value.Status is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Status);
    }
}

public sealed class AddressSubscriptionResultJsonConverter : JsonConverter<ElectrumXAddressSubscription>
{
    public override bool HandleNull => true;

    public override ElectrumXAddressSubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new ElectrumXAddressSubscription(reader.GetString()),
            JsonTokenType.Null => new ElectrumXAddressSubscription(null),
            _ => throw new JsonException("The result must be a string or null.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXAddressSubscription value,
        JsonSerializerOptions options)
    {
        if (value.Status is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Status);
    }
}

public sealed class ScriptHashStatusResultJsonConverter : JsonConverter<ElectrumXScriptHashStatus>
{
    public override bool HandleNull => true;

    public override ElectrumXScriptHashStatus Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new ElectrumXScriptHashStatus(reader.GetString()),
            JsonTokenType.Null => new ElectrumXScriptHashStatus(null),
            _ => throw new JsonException("The result must be a string or null.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXScriptHashStatus value,
        JsonSerializerOptions options)
    {
        if (value.Status is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Status);
    }
}

public sealed class AddressUnsubscriptionResultJsonConverter : JsonConverter<ElectrumXAddressUnsubscription>
{
    public override ElectrumXAddressUnsubscription Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new ElectrumXAddressUnsubscription(true),
            JsonTokenType.False => new ElectrumXAddressUnsubscription(false),
            _ => throw new JsonException("The result must be a boolean.")
        };
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXAddressUnsubscription value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.WasUnsubscribed);
    }
}

public sealed class TransactionUnsubscriptionResultJsonConverter : JsonConverter<ElectrumXTransactionUnsubscription>
{
    public override ElectrumXTransactionUnsubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new ElectrumXTransactionUnsubscription(true),
            JsonTokenType.False => new ElectrumXTransactionUnsubscription(false),
            _ => throw new JsonException("The result must be a boolean.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXTransactionUnsubscription value,
        JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.WasUnsubscribed);
    }
}

public sealed class DoubleSpendProofUnsubscriptionResultJsonConverter : JsonConverter<ElectrumXDoubleSpendProofUnsubscription>
{
    public override ElectrumXDoubleSpendProofUnsubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new ElectrumXDoubleSpendProofUnsubscription(true),
            JsonTokenType.False => new ElectrumXDoubleSpendProofUnsubscription(false),
            _ => throw new JsonException("The result must be a boolean.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXDoubleSpendProofUnsubscription value,
        JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.WasUnsubscribed);
    }
}

public sealed class HeadersUnsubscriptionResultJsonConverter : JsonConverter<ElectrumXHeadersUnsubscription>
{
    public override ElectrumXHeadersUnsubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => new ElectrumXHeadersUnsubscription(true),
            JsonTokenType.False => new ElectrumXHeadersUnsubscription(false),
            _ => throw new JsonException("The result must be a boolean.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXHeadersUnsubscription value,
        JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value.WasUnsubscribed);
    }
}

public sealed class TransactionHeightResultJsonConverter : JsonConverter<ElectrumXTransactionHeightResult>
{
    public override bool HandleNull => true;

    public override ElectrumXTransactionHeightResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetInt64(out long height) => new ElectrumXTransactionHeightResult(height),
            JsonTokenType.Null => new ElectrumXTransactionHeightResult(null),
            _ => throw new JsonException("The result must be an integer or null.")
        };
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXTransactionHeightResult value, JsonSerializerOptions options)
    {
        if (value.Height is {} height)
        {
            writer.WriteNumberValue(height);
            return;
        }

        writer.WriteNullValue();
    }
}

public sealed class TransactionSubscriptionResultJsonConverter : JsonConverter<ElectrumXTransactionSubscription>
{
    public override bool HandleNull => true;

    public override ElectrumXTransactionSubscription Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number when reader.TryGetInt64(out long height) => new ElectrumXTransactionSubscription(height),
            JsonTokenType.Null => new ElectrumXTransactionSubscription(null),
            _ => throw new JsonException("The result must be an integer or null.")
        };
    }

    public override void Write(
        Utf8JsonWriter writer,
        ElectrumXTransactionSubscription value,
        JsonSerializerOptions options)
    {
        if (value.Height is {} height)
        {
            writer.WriteNumberValue(height);
            return;
        }

        writer.WriteNullValue();
    }
}

public sealed class StringListResultJsonConverter : JsonConverter<ElectrumXStringList>
{
    public override ElectrumXStringList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The result must be an array of strings.");
        }

        var values = new List<string>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("A result array item must be a string.");
            }

            values.Add(reader.GetString() ?? throw new JsonException("A result array item cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The result array is incomplete.");
        }

        return new ElectrumXStringList(values);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXStringList value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (string item in value.Value)
        {
            writer.WriteStringValue(item);
        }

        writer.WriteEndArray();
    }
}

public sealed class HistoryListResultJsonConverter : JsonConverter<ElectrumXHistoryList>
{
    public override ElectrumXHistoryList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The result must be a transaction-history array.");
        }

        var values = new List<ElectrumXHistoryEntry>();
        var converter = (JsonConverter<ElectrumXHistoryEntry>)options.GetConverter(typeof(ElectrumXHistoryEntry));
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            values.Add(converter.Read(ref reader, typeof(ElectrumXHistoryEntry), options)
                       ?? throw new JsonException("A transaction-history item cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The result array is incomplete.");
        }

        return new ElectrumXHistoryList(values);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXHistoryList value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        var converter = (JsonConverter<ElectrumXHistoryEntry>)options.GetConverter(typeof(ElectrumXHistoryEntry));
        foreach (var item in value.Value)
        {
            converter.Write(writer, item, options);
        }

        writer.WriteEndArray();
    }
}

public sealed class MempoolEntryListResultJsonConverter : JsonConverter<ElectrumXMempoolEntryList>
{
    public override ElectrumXMempoolEntryList Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The result must be a mempool-entry array.");
        }

        var values = new List<ElectrumXMempoolEntry>();
        var converter = (JsonConverter<ElectrumXMempoolEntry>)options.GetConverter(typeof(ElectrumXMempoolEntry));
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            values.Add(converter.Read(ref reader, typeof(ElectrumXMempoolEntry), options)
                       ?? throw new JsonException("A mempool entry cannot be null."));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The result array is incomplete.");
        }

        return new ElectrumXMempoolEntryList(values);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXMempoolEntryList value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        var converter = (JsonConverter<ElectrumXMempoolEntry>)options.GetConverter(typeof(ElectrumXMempoolEntry));
        foreach (var item in value.Value)
        {
            converter.Write(writer, item, options);
        }

        writer.WriteEndArray();
    }
}
