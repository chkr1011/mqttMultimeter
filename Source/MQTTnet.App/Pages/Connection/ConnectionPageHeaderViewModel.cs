using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Connection;

public sealed class ConnectionPageHeaderViewModel : BaseViewModel
{
    public bool IsConnected
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }
}