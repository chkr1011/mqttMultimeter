using System;
using System.Globalization;
using Avalonia.Data;
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

    public sealed class RadioButtonValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true ? parameter : null!;
        }
    }

    public static class DefaultConverters
    {
        public static BooleanInverter BooleanInverter { get; } = new BooleanInverter();

        public static RadioButtonValueConverter RadioButtonValueConverter { get; } = new RadioButtonValueConverter();
    }
}
