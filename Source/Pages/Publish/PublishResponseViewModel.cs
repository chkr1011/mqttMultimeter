using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using MQTTnet;
using ReactiveUI;

namespace mqttMultimeter.Pages.Publish;

public sealed class PublishResponseViewModel : BaseViewModel
{
    ushort? _packetIdentifier;
    int? _reasonCode;
    string? _reasonCodeText;
    string? _reasonString;

    public ushort? PacketIdentifier
    {
        get => _packetIdentifier;
        set => this.RaiseAndSetIfChanged(ref _packetIdentifier, value);
    }

    public int? ReasonCode
    {
        get => _reasonCode;
        set => this.RaiseAndSetIfChanged(ref _reasonCode, value);
    }

    public string? ReasonCodeText
    {
        get => _reasonCodeText;
        set => this.RaiseAndSetIfChanged(ref _reasonCodeText, value);
    }

    public string? ReasonString
    {
        get => _reasonString;
        set => this.RaiseAndSetIfChanged(ref _reasonString, value);
    }

    public UserPropertiesViewModel UserProperties { get; } = new();

    public void ApplyResponse(MqttClientPublishResult response)
    {
        ReasonCode = (int)response.ReasonCode;
        ReasonCodeText = response.ReasonCode.ToString();
        ReasonString = response.ReasonString;
        PacketIdentifier = response.PacketIdentifier;
        UserProperties.Load(response.UserProperties);
    }
}