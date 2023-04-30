using MQTTnet.Protocol;

namespace mqttMultimeter.Pages.Subscriptions.State;

public sealed class SubscriptionState
{
    public string? Name { get; set; }

    public bool NoLocal { get; set; }

    public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

    public bool RetainAsPublished { get; set; }

    public MqttRetainHandling RetainHandling { get; set; }

    public string? Topic { get; set; }
}