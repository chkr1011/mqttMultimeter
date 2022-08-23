using System;
using MQTTnet;
using MQTTnetApp.Common;
using MQTTnetApp.Pages.Inflight;

namespace MQTTnetApp.Pages.TopicExplorer;

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
        Retain = applicationMessage.Retain;

        Delay = delay;
        InflightItem = InflightPageItemViewModel.Create(applicationMessage, 0);
    }

    public TimeSpan Delay { get; init; }

    public InflightPageItemViewModel InflightItem { get; init; }

    public string Payload { get; init; }

    public bool Retain { get; init; }

    public DateTime Timestamp { get; init; }
}