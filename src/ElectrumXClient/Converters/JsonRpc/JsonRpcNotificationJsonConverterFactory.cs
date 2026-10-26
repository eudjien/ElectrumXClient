using NorthGomel.ElectrumXClient.Abstractions.JsonRpc;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.JsonRpc;

internal sealed class JsonRpcNotificationJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
               typeToConvert.GetGenericTypeDefinition() == typeof(JsonRpcNotification<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var dataType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(JsonRpcNotificationJsonConverter<>).MakeGenericType(dataType);
        return (JsonConverter)Activator.CreateInstance(converterType);
    }
}
