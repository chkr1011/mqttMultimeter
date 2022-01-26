using System;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MQTTnet.Protocol;

namespace MQTTnet.App.Converters;

public static class EnumConverters
{
    public static readonly IValueConverter ToEnumString = new FuncValueConverter<Enum, string>(Convert.ToString);

    public static readonly IValueConverter ToEnumValue = new FuncValueConverter<Enum, int>(Convert.ToInt32);

    public static readonly IValueConverter ExpandQualityOfServiceValue = new FuncValueConverter<MqttQualityOfServiceLevel?, string>(v =>
    {
        if (!v.HasValue)
        {
            return string.Empty;
        }

        return $"{Convert.ToString(v)} ({Convert.ToInt32(v)})";
    });

    public static readonly IValueConverter BooleanToTextWrapping = new FuncValueConverter<bool?, TextWrapping>(v =>
    {
        if (!v.HasValue)
        {
            return TextWrapping.NoWrap;
        }

        return v.Value ? TextWrapping.Wrap : TextWrapping.NoWrap;
    });
}