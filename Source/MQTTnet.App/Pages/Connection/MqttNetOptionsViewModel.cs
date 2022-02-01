using MQTTnet.App.Common;
using ReactiveUI;

namespace MQTTnet.App.Pages.Connection;

public sealed class MqttNetOptionsViewModel : BaseViewModel
{
    bool _enableLogging;
    bool _enablePacketInspection;

    public bool EnableLogging
    {
        get => _enableLogging;
        set => this.RaiseAndSetIfChanged(ref _enableLogging, value);
    }

    public bool EnablePacketInspection
    {
        get => _enablePacketInspection;
        set => this.RaiseAndSetIfChanged(ref _enablePacketInspection, value);
    }
}