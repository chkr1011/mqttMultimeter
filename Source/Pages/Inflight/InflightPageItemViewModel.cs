using System;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnetApp.Controls;

namespace MQTTnetApp.Pages.Inflight;

public sealed class InflightPageItemViewModel
{
    public event EventHandler? DeleteRetainedMessageRequested;

    public event EventHandler? RepeatMessageRequested;

    public string ContentType { get; init; } = string.Empty;

    public long Length { get; init; }

    public long Number { get; init; }

    public byte[] Payload { get; init; } = Array.Empty<byte>();

    public string PayloadPreview { get; set; } = string.Empty;

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; init; }

    public bool Retain { get; init; }

    public MqttApplicationMessage? Source { get; init; }

    public DateTime Timestamp { get; init; }

    public string Topic { get; init; } = string.Empty;

    public UserPropertiesViewModel UserProperties { get; } = new();

    public static InflightPageItemViewModel Create(MqttApplicationMessage applicationMessage, long number)
    {
        var itemViewModel = new InflightPageItemViewModel
        {
            Timestamp = DateTime.Now,
            Number = number,
            Topic = applicationMessage.Topic,
            Length = applicationMessage.Payload?.Length ?? 0L,
            Retain = applicationMessage.Retain,
            Source = applicationMessage,
            Payload = applicationMessage.Payload ?? Array.Empty<byte>(),
            ContentType = applicationMessage.ContentType,
            QualityOfServiceLevel = applicationMessage.QualityOfServiceLevel,
            UserProperties =
            {
                IsReadOnly = true
            }
        };

        itemViewModel.UserProperties.Load(applicationMessage.UserProperties);

        return itemViewModel;
    }

    public void DeleteRetainedMessage()
    {
        DeleteRetainedMessageRequested?.Invoke(this, EventArgs.Empty);
    }

    public void RepeatMessage()
    {
        RepeatMessageRequested?.Invoke(this, EventArgs.Empty);
    }
}