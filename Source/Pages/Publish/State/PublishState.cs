using MQTTnet.Protocol;

namespace MQTTnetApp.Pages.Publish.State;

public sealed class PublishState
{
    public string? ContentType { get; set; }
    
    public string? Name { get; set; }

    public MqttPayloadFormatIndicator PayloadFormatIndicator { get; set; }

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

    public bool Retain { get; set; }

    public string? Topic { get; set; }
}