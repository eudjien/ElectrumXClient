using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

public sealed class JsonRpcResponseJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(JsonRpcResponse<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var dataType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(JsonRpcResponseJsonConverter<>).MakeGenericType(dataType);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}
