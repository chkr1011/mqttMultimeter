using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using MQTTnet;
using ReactiveUI;

namespace mqttMultimeter.Pages.Connection;

public sealed class ConnectResponseViewModel : BaseViewModel
{
    bool? _isSessionPresent;
    int? _reasonCode;
    string _reasonCodeText = string.Empty;
    string _reasonString = string.Empty;

    public bool? IsSessionPresent
    {
        get => _isSessionPresent;
        set => this.RaiseAndSetIfChanged(ref _isSessionPresent, value);
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

    public void ApplyResponse(MqttClientConnectResult response)
    {
        ReasonCode = (int)response.ResultCode;
        ReasonCodeText = response.ResultCode.ToString();
        ReasonString = response.ReasonString;
        IsSessionPresent = response.IsSessionPresent;
        UserProperties.Load(response.UserProperties);
    }
}