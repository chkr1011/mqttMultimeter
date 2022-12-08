using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace mqttMultimeter.Converters;

public sealed class TimeSpanToLongStringConverter : IValueConverter
{
    public static TimeSpanToLongStringConverter Instance { get; } = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!(value is TimeSpan timeSpan))
        {
            return "-";
        }

        return timeSpan.ToString(@"hh\:mm\:ss\:ffff");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}