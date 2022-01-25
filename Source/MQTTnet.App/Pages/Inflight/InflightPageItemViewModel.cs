using System;
using MQTTnet.App.Controls;

namespace MQTTnet.App.Pages.Inflight;

public sealed class InflightPageItemViewModel
{
    public long Length { get; init; }

    public int Number { get; init; }

    public bool Retained { get; init; }

    public MqttApplicationMessage Source { get; init; }
    public DateTime Timestamp { get; init; }

    public string? Topic { get; init; }

    public UserPropertiesViewModel UserProperties { get; } = new();
}