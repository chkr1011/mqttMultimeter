using System;
using MQTTnet;

namespace mqttMultimeter.Pages.Inflight;

public static class InflightPageItemViewModelFactory
{
    public static InflightPageItemViewModel Create(MqttApplicationMessage message, long number)
    {
        ArgumentNullException.ThrowIfNull(message);

        var itemViewModel = new InflightPageItemViewModel(message)
        {
            Timestamp = DateTime.Now,
            Number = number
        };

        itemViewModel.UserProperties.Load(message.UserProperties);

        return itemViewModel;
    }
}