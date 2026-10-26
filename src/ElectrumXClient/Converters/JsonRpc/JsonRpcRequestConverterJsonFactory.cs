using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

public sealed class JsonRpcRequestConverterJsonFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(JsonRpcRequest<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var dataType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(JsonRpcRequestConverter<>).MakeGenericType(dataType);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}
