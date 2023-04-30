using System;
using System.Collections.Generic;
using Avalonia.Data.Converters;
using Avalonia.Media;
using MQTTnet.Protocol;

namespace mqttMultimeter.Converters;

public static class EnumConverters
{
    public static readonly IValueConverter ExpandQualityOfServiceValue = new FuncValueConverter<MqttQualityOfServiceLevel?, string>(v =>
    {
        if (!v.HasValue)
        {
            return string.Empty;
        }

        return $"{Convert.ToString(v)} ({Convert.ToInt32(v)})";
    });

    public static readonly IValueConverter ExpandSubscriptionIdentifiers = new FuncValueConverter<IEnumerable<uint>?, string>(v =>
    {
        if (v == null)
        {
            return string.Empty;
        }

        return string.Join(", ", v);
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