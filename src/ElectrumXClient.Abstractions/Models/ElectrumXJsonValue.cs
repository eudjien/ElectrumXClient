using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace NorthGomel.ElectrumXClient.Abstractions.Models;

/// <summary>Represents a JSON value without coupling protocol models to a serializer library.</summary>
public abstract class ElectrumXJsonValue
{
    private protected ElectrumXJsonValue()
    {
    }

    public static implicit operator ElectrumXJsonValue(string value)
    {
        return new StringValue(value);
    }

    /// <summary>A JSON string value.</summary>
    public sealed class StringValue : ElectrumXJsonValue
    {
        /// <summary>Creates a JSON string value.</summary>
        public StringValue(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>Gets the string contents.</summary>
        public string Value { get; }
    }

    /// <summary>A JSON number preserving its exact textual representation.</summary>
    public sealed class NumberValue : ElectrumXJsonValue
    {
        private NumberValue(string rawJson)
        {
            RawJson = rawJson;
        }

        /// <summary>Gets the validated JSON number text.</summary>
        public string RawJson { get; }

        /// <summary>Creates a JSON number from a 64-bit integer.</summary>
        public static NumberValue FromInt64(long value)
        {
            return new NumberValue(value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>Creates a JSON number from a decimal.</summary>
        public static NumberValue FromDecimal(decimal value)
        {
            return new NumberValue(value.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>Creates a number value from valid JSON number text without losing precision.</summary>
        public static NumberValue FromRawJson(string rawJson)
        {
            if (rawJson is null)
            {
                throw new ArgumentNullException(nameof(rawJson));
            }

            if (!IsValidJsonNumber(rawJson))
            {
                throw new ArgumentException("The value must be a valid JSON number.", nameof(rawJson));
            }

            return new NumberValue(rawJson);
        }

        private static bool IsValidJsonNumber(string value)
        {
            int index = 0;

            if (index < value.Length && value[index] == '-')
            {
                index++;
            }

            if (index >= value.Length)
            {
                return false;
            }

            switch (value[index])
            {
                case '0':
                    {
                        index++;
                        if (index < value.Length && IsDigit(value[index]))
                        {
                            return false;
                        }
                        break;
                    }
                case >= '1' and <= '9':
                    {
                        do
                        {
                            index++;
                        } while (index < value.Length && IsDigit(value[index]));
                        break;
                    }
                default:
                    return false;
            }

            if (index < value.Length && value[index] == '.')
            {
                index++;
                int fractionStart = index;
                while (index < value.Length && IsDigit(value[index]))
                {
                    index++;
                }

                if (index == fractionStart)
                {
                    return false;
                }
            }

            if (index < value.Length && value[index] is 'e' or 'E')
            {
                index++;
                if (index < value.Length && value[index] is '+' or '-')
                {
                    index++;
                }

                int exponentStart = index;
                while (index < value.Length && IsDigit(value[index]))
                {
                    index++;
                }

                if (index == exponentStart)
                {
                    return false;
                }
            }

            return index == value.Length;
        }

        private static bool IsDigit(char character)
        {
            return character is >= '0' and <= '9';
        }
    }

    /// <summary>A JSON boolean value.</summary>
    public sealed class BooleanValue : ElectrumXJsonValue
    {
        /// <summary>Creates a JSON boolean value.</summary>
        public BooleanValue(bool value)
        {
            Value = value;
        }

        /// <summary>Gets the boolean contents.</summary>
        public bool Value { get; }
    }

    /// <summary>The JSON null value.</summary>
    public sealed class NullValue : ElectrumXJsonValue
    {

        private NullValue()
        {
        }

        /// <summary>Gets the singleton JSON null value.</summary>
        public static NullValue Instance { get; } = new();
    }

    /// <summary>A JSON array value.</summary>
    public sealed class ArrayValue : ElectrumXJsonValue
    {
        /// <summary>Creates a JSON array value.</summary>
        public ArrayValue(IReadOnlyList<ElectrumXJsonValue> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var itemCopy = new ElectrumXJsonValue[items.Count];
            for (int index = 0; index < items.Count; index++)
            {
                itemCopy[index] = items[index]
                                  ?? throw new ArgumentException("JSON array items cannot be null.", nameof(items));
            }

            Items = Array.AsReadOnly(itemCopy);
        }

        /// <summary>Gets the array items.</summary>
        public IReadOnlyList<ElectrumXJsonValue> Items { get; }
    }

    /// <summary>A JSON object value.</summary>
    public sealed class ObjectValue : ElectrumXJsonValue
    {
        /// <summary>Creates a JSON object value.</summary>
        public ObjectValue(IReadOnlyDictionary<string, ElectrumXJsonValue> properties)
        {
            if (properties is null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            var propertyCopy = new Dictionary<string, ElectrumXJsonValue>(StringComparer.Ordinal);
            foreach ((string name, var value) in properties)
            {
                if (name is null)
                {
                    throw new ArgumentException("JSON object property names cannot be null.", nameof(properties));
                }

                propertyCopy.Add(
                    name,
                    value ?? throw new ArgumentException("JSON object property values cannot be null.", nameof(properties)));
            }

            Properties = new ReadOnlyDictionary<string, ElectrumXJsonValue>(propertyCopy);
        }

        /// <summary>Gets the object properties.</summary>
        public IReadOnlyDictionary<string, ElectrumXJsonValue> Properties { get; }
    }
}
