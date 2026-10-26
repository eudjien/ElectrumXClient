using NorthGomel.ElectrumXClient.Abstractions.Models.Results;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results;

internal sealed class ElectrumXResultAliasJsonConverter<TBase, TResult>(
    Func<TBase, TResult> fromBase,
    Func<TResult, TBase> toBase) : JsonConverter<TResult>
    where TBase : ElectrumXResult
    where TResult : ElectrumXResult
{
    public override TResult Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var baseConverter = (JsonConverter<TBase>)options.GetConverter(typeof(TBase));
        var baseResult = baseConverter.Read(ref reader, typeof(TBase), options)
                         ?? throw new JsonException("The result cannot be null.");

        return fromBase(baseResult);
    }

    public override void Write(Utf8JsonWriter writer, TResult value, JsonSerializerOptions options)
    {
        var baseConverter = (JsonConverter<TBase>)options.GetConverter(typeof(TBase));
        baseConverter.Write(writer, toBase(value), options);
    }
}
