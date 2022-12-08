using System;
using mqttMultimeter.Common;
using mqttMultimeter.Pages.Inflight;
using MQTTnet;

namespace mqttMultimeter.Pages.TopicExplorer;

public sealed class TopicExplorerItemMessageViewModel : BaseViewModel
{
    public TopicExplorerItemMessageViewModel(DateTime timestamp, MqttApplicationMessage applicationMessage, string payload, TimeSpan delay)
    {
        if (applicationMessage == null)
        {
            throw new ArgumentNullException(nameof(applicationMessage));
        }

        Timestamp = timestamp;
        Payload = payload ?? throw new ArgumentNullException(nameof(payload));
        PayloadLength = applicationMessage.Payload?.Length ?? 0;
        Retain = applicationMessage.Retain;

        Delay = delay;
        InflightItem = InflightPageItemViewModelFactory.Create(applicationMessage, 0);
    }

    public TimeSpan Delay { get; init; }

    public InflightPageItemViewModel InflightItem { get; init; }

    public string Payload { get; init; }

    public int PayloadLength { get; }

    public bool Retain { get; }

    public DateTime Timestamp { get; init; }
}