using System;
using System.Globalization;

namespace MQTTnet.App.Common.BufferInspector
{
    public sealed class BufferValueViewModel : BaseViewModel
    {
        string _value;

        public BufferValueViewModel(string name, string value = "")
        {
            Name = name;
            _value = value;
        }

        public string Name { get; }

        public void SetValue(object value)
        {
            Value = Convert.ToString(value, CultureInfo.InvariantCulture)?.Trim() ?? string.Empty;
        }

        public string Value
        {
            get => _value;
            private set
            {
                _value = value;
                OnPropertyChanged();
            }
        }
    }
}