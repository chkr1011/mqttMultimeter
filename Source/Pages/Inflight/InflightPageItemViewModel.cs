using System;
using System.Buffers;
using System.Collections.Generic;
using mqttMultimeter.Controls;
using MQTTnet;
using MQTTnet.Protocol;

namespace mqttMultimeter.Pages.Inflight;

public sealed class InflightPageItemViewModel
{
    public InflightPageItemViewModel(MqttApplicationMessage message)
    {
        Message = message ?? throw new ArgumentNullException(nameof(message));

        if (message.Payload.Length > 0)
        {
            Payload = message.Payload.ToArray();
        }
        else
        {
            Payload = [];
        }

        Length = Payload.Length;
        UserProperties.IsReadOnly = true;
    }

    public event EventHandler? DeleteRetainedMessageRequested;

    public event EventHandler? RepeatMessageRequested;

    public string? ContentType => Message.ContentType;

    public byte[]? CorrelationData => Message.CorrelationData;

    public bool Dup => Message.Dup;

    public long Length { get; }

    public MqttApplicationMessage Message { get; }

    public uint MessageExpiryInterval => Message.MessageExpiryInterval;

    public long Number { get; init; }

    public byte[] Payload { get; }

    public MqttPayloadFormatIndicator PayloadFormatIndicator => Message.PayloadFormatIndicator;

    public MqttQualityOfServiceLevel QualityOfServiceLevel => Message.QualityOfServiceLevel;

    public string ResponseTopic => Message.ResponseTopic;

    public bool Retain => Message.Retain;

    public List<uint>? SubscriptionIdentifiers => Message.SubscriptionIdentifiers;

    public DateTime Timestamp { get; init; }

    public string Topic => Message.Topic;

    public UserPropertiesViewModel UserProperties { get; } = new();

    public void DeleteRetainedMessage()
    {
        DeleteRetainedMessageRequested?.Invoke(this, EventArgs.Empty);
    }

    public void RepeatMessage()
    {
        RepeatMessageRequested?.Invoke(this, EventArgs.Empty);
    }
}