using MQTTnet.App.Common;
using MQTTnet.Client;

namespace MQTTnet.App.Pages.Publish;

public sealed class PublishResponseViewModel : BaseViewModel
{
    public string ReasonCode
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public string ReasonString
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public void ApplyResponse(MqttClientPublishResult response)
    {
        ReasonCode = response.ReasonCode.ToString();
        ReasonString = response.ReasonString;
    }
}