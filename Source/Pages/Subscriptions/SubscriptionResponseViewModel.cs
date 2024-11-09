using System.Linq;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using MQTTnet;
using ReactiveUI;

namespace mqttMultimeter.Pages.Subscriptions;

public sealed class SubscriptionResponseViewModel : BaseViewModel
{
    uint? _packetIdentifier;
    int? _reasonCode;
    string _reasonCodeText = string.Empty;
    string _reasonString = string.Empty;

    public SubscriptionResponseViewModel()
    {
        UserProperties.IsReadOnly = true;
    }

    public uint? PacketIdentifier
    {
        get => _packetIdentifier;
        set => this.RaiseAndSetIfChanged(ref _packetIdentifier, value);
    }

    public int? ReasonCode
    {
        get => _reasonCode;
        set => this.RaiseAndSetIfChanged(ref _reasonCode, value);
    }

    public string ReasonCodeText
    {
        get => _reasonCodeText;
        set => this.RaiseAndSetIfChanged(ref _reasonCodeText, value);
    }

    public string ReasonString
    {
        get => _reasonString;
        set => this.RaiseAndSetIfChanged(ref _reasonString, value);
    }

    public UserPropertiesViewModel UserProperties { get; } = new();

    public void ApplyResponse(MqttClientSubscribeResult response)
    {
        ReasonCodeText = response.Items.First().ResultCode.ToString();
        ReasonCode = (int)response.Items.First().ResultCode;
        ReasonString = response.ReasonString;
        PacketIdentifier = response.PacketIdentifier;
        UserProperties.Load(response.UserProperties);
    }

    public void ApplyResponse(MqttClientUnsubscribeResult response)
    {
        ReasonCodeText = response.Items.First().ResultCode.ToString();
        ReasonCode = (int)response.Items.First().ResultCode;
        ReasonString = response.ReasonString;
        PacketIdentifier = response.PacketIdentifier;
        UserProperties.Load(response.UserProperties);
    }
}