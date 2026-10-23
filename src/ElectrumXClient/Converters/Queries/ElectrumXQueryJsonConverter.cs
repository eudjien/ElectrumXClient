using NorthGomel.ElectrumXClient.Abstractions.Models.Queries;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Queries;

/// <summary>Base converter for ElectrumX queries, which are write-only.</summary>
/// <typeparam name="TQuery">The query model type.</typeparam>
public abstract class ElectrumXQueryJsonConverter<TQuery> : JsonConverter<TQuery>
    where TQuery : ElectrumXQuery
{
    public sealed override TQuery Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Deserializing ElectrumX queries is not supported.");
    }
}
