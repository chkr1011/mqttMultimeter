using System.Collections.Generic;
using MQTTnet.Packets;
using MQTTnet.Protocol;

namespace mqttMultimeter.Pages.Inflight.Export.Model;

public sealed class InflightPageExportMessage
{
    public string? ContentType { get; set; }

    public byte[]? CorrelationData { get; set; }

    public bool Dup { get; set; }

    public uint MessageExpiryInterval { get; set; }

    public byte[]? Payload { get; set; }

    public MqttPayloadFormatIndicator PayloadFormatIndicator { get; set; } = MqttPayloadFormatIndicator.Unspecified;

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

    public string? ResponseTopic { get; set; }

    public bool Retain { get; set; }

    public List<uint>? SubscriptionIdentifiers { get; set; }

    public string? Topic { get; set; }

    public ushort TopicAlias { get; set; }

    public List<MqttUserProperty>? UserProperties { get; set; }
}