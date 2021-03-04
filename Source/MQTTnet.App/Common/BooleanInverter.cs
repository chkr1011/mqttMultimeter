using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MQTTnet.App.Common
{
    public sealed class BooleanInverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is true);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is true);
        }
    }
}