using NorthGomel.ElectrumXClient.Abstractions;
using NorthGomel.ElectrumXClient.Abstractions.Models.Results.Server;
using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NorthGomel.ElectrumXClient.Converters.Results.Server;

public sealed class ServerVersionResultJsonConverter : JsonConverter<ElectrumXServerVersion>
{
    public override bool HandleNull => true;

    public override ElectrumXServerVersion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string legacySoftwareVersion = reader.GetString()
                                           ?? throw new JsonException("The server version response must be a string.");

            return new ElectrumXServerVersion(legacySoftwareVersion, new ElectrumXProtocolVersion(1, 0));
        }

        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("The server version response must be a string or an array of two strings.");
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The server version response must contain exactly two strings.");
        }

        string softwareVersion = reader.GetString()
                                 ?? throw new JsonException("The server version response must contain exactly two strings.");

        if (!reader.Read() || reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("The server version response must contain exactly two strings.");
        }

        string protocolVersionValue = reader.GetString()
                                      ?? throw new JsonException("The server version response must contain exactly two strings.");

        int patch = -1;
        string[] versionComponents = protocolVersionValue.Split('.');
        if (versionComponents.Length is < 2 or > 3 ||
            !int.TryParse(versionComponents[0], NumberStyles.None, CultureInfo.InvariantCulture, out int major) ||
            !int.TryParse(versionComponents[1], NumberStyles.None, CultureInfo.InvariantCulture, out int minor) ||
            versionComponents.Length == 3 &&
            !int.TryParse(versionComponents[2], NumberStyles.None, CultureInfo.InvariantCulture, out patch))
        {
            throw new JsonException("The server version response must contain a valid protocol version.");
        }

        var protocolVersion = versionComponents.Length == 3
            ? new ElectrumXProtocolVersion(major, minor, patch)
            : new ElectrumXProtocolVersion(major, minor);

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("The server version response must contain exactly two strings.");
        }

        return new ElectrumXServerVersion(softwareVersion, protocolVersion);
    }

    public override void Write(Utf8JsonWriter writer, ElectrumXServerVersion value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartArray();
        writer.WriteStringValue(value.SoftwareVersion);
        writer.WriteStringValue(value.ProtocolVersion.ToString());
        writer.WriteEndArray();
    }
}
