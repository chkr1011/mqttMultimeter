using MQTTnet.App.Common;
using MQTTnet.App.Controls.UserProperties;
using MQTTnet.Client;

namespace MQTTnet.App.Pages.Publish;

public sealed class PublishResponseViewModel : BaseViewModel
{
    public ushort? PacketIdentifier
    {
        get => GetValue<ushort?>();
        set => SetValue(value);
    }

    public int? ReasonCode
    {
        get => GetValue<int?>();
        set => SetValue(value);
    }

    public string ReasonCodeText
    {
        get => GetValue<string>();
        set => SetValue(value);
    }
    
    public string ReasonString
    {
        get => GetValue<string>();
        set => SetValue(value);
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