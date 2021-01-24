using System.Collections.Generic;

namespace MQTTnet.App.Common
{
    internal sealed class ViewModelPropertyStore
    {
        readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public bool SetValue<TValue>(string propertyName, TValue value)
        {
            var equalityComparer = EqualityComparer<TValue>.Default;

            if (_values.TryGetValue(propertyName, out var existingValue))
            {
                if (equalityComparer.Equals(value, (TValue)existingValue))
                {
                    // The value already exists and has the same value.
                    return false;
                }
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

        public TValue GetValue<TValue>(string propertyName)
        {
            if (_values.TryGetValue(propertyName, out var value))
            {
                return (TValue)value;
            }

            return default!;
        }
    }
}