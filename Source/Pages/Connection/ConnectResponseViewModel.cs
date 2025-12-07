using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using MQTTnet;
using ReactiveUI;

namespace mqttMultimeter.Pages.Connection;

public sealed class ConnectResponseViewModel : BaseViewModel
{
    public ConnectResponseViewModel()
    {
        ReasonCodeText = string.Empty;
        ReasonString = string.Empty;
    }

    public bool? IsSessionPresent
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public int? ReasonCode
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string ReasonCodeText
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string ReasonString
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public UserPropertiesViewModel UserProperties { get; } = new();

    public void ApplyResponse(MqttClientConnectResult response)
    {
        ReasonCode = (int)response.ResultCode;
        ReasonCodeText = response.ResultCode.ToString();
        ReasonString = response.ReasonString;
        IsSessionPresent = response.IsSessionPresent;
        UserProperties.Load(response.UserProperties);
    }
}