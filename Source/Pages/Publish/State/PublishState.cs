using System.Collections.Generic;
using mqttMultimeter.Services.State.Model;
using MQTTnet.Protocol;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace mqttMultimeter.Pages.Publish.State;

public sealed class PublishState
{
    public string? ContentType { get; set; }

    public uint MessageExpiryInterval { get; set; }

    public string? Name { get; set; }

    public string? Payload { get; set; }

    public MqttPayloadFormatIndicator PayloadFormatIndicator { get; set; }

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

    public string? ResponseTopic { get; set; }

    public bool Retain { get; set; }

    public uint SubscriptionIdentifier { get; set; }

    public string? Topic { get; set; }

    public ushort TopicAlias { get; set; }

    public List<UserProperty> UserProperties { get; set; } = new();
}