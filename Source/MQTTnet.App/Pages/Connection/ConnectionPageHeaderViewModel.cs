using MQTTnet.App.Common;
using ReactiveUI;

namespace MQTTnet.App.Pages.Connection;

public sealed class ConnectionPageHeaderViewModel : BaseViewModel
{
    bool _isConnected;
    
    public bool IsConnected
    {
        get => _isConnected;
        set => this.RaiseAndSetIfChanged(ref _isConnected, value);
    }
}