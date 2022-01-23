using System.Collections.Generic;

namespace MQTTnet.App.Common;

sealed class ViewModelPropertyStore
{
    readonly Dictionary<string, object> _values = new();

    public TValue GetValue<TValue>(string propertyName)
    {
        if (_values.TryGetValue(propertyName, out var value))
        {
            return (TValue)value;
        }

        return default!;
    }

    public bool SetValue<TValue>(string propertyName, TValue value)
    {
        if (!_values.TryGetValue(propertyName, out var existingValue))
        {
            // The value is new. No further checks are required.
            _values[propertyName] = value!;
            return true;
        }

        var equalityComparer = EqualityComparer<TValue>.Default;

        if (equalityComparer.Equals(value, (TValue)existingValue))
        {
            // The value already exists and has the same value.
            return false;
        }

        if (equalityComparer.Equals(value, default))
        {
            // We do not store default values in the dictionary.
            _values.Remove(propertyName);
            return true;
        }

        _values[propertyName] = value!;
        return true;
    }
}