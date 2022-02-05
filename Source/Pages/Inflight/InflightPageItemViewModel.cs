using System;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnetApp.Controls;

namespace MQTTnetApp.Pages.Inflight;

public sealed class InflightPageItemViewModel
{
    public InflightPageItemViewModel(InflightPageViewModel ownerPage)
    {
        OwnerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));
    }
    
    public long Length { get; init; }

    public int Number { get; init; }

    public InflightPageViewModel OwnerPage { get; }

    public string PayloadPreview { get; set; } = string.Empty;

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; init; }

    public bool Retained { get; init; }

    public MqttApplicationMessage Source { get; init; }

    public DateTime Timestamp { get; init; }

    public string Topic { get; init; }

    public UserPropertiesViewModel UserProperties { get; } = new();

    public void Repeat()
    {
        OwnerPage.RepeatItem(this);
    }
}