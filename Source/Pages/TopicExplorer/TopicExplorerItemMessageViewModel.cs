using System;
using System.Text;
using MQTTnet;
using MQTTnetApp.Common;
using MQTTnetApp.Pages.Inflight;

namespace MQTTnetApp.Pages.TopicExplorer;

public sealed class TopicExplorerItemMessageViewModel : BaseViewModel
{
    public TopicExplorerItemMessageViewModel(MqttApplicationMessage applicationMessage)
    {
        Timestamp = DateTime.Now;

        try
        {
            PayloadString = Encoding.UTF8.GetString(applicationMessage.Payload ?? ReadOnlySpan<byte>.Empty);
        }
        catch
        {
            // Ignore error.
            PayloadString = string.Empty;
        }

        InflightItem = InflightPageItemViewModel.Create(applicationMessage, 0);
    }

    public InflightPageItemViewModel InflightItem { get; }

    public string PayloadString { get; }

    public DateTime Timestamp { get; }
}