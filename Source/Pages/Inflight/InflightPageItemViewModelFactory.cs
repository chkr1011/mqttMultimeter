using System;
using MQTTnet;

namespace mqttMultimeter.Pages.Inflight;

public static class InflightPageItemViewModelFactory
{
    public static InflightPageItemViewModel Create(MqttApplicationMessage message, long number)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var itemViewModel = new InflightPageItemViewModel(message)
        {
            Timestamp = DateTime.Now,
            Number = number
        };

        itemViewModel.UserProperties.Load(message.UserProperties);

        return itemViewModel;
    }
}