using System.Collections.ObjectModel;
using MQTTnet.App.Common;
using MQTTnet.Formatter;
using ReactiveUI;

namespace MQTTnet.App.Pages.Connection;

public sealed class ProtocolOptionsViewModel : BaseViewModel
{
    ProtocolVersionViewModel? _selectedProtocolVersion;

    public ProtocolOptionsViewModel()
    {
        ProtocolVersions.Add(new ProtocolVersionViewModel("3.1.0", MqttProtocolVersion.V310));
        ProtocolVersions.Add(new ProtocolVersionViewModel("3.1.1", MqttProtocolVersion.V311));
        ProtocolVersions.Add(new ProtocolVersionViewModel("5.0.0", MqttProtocolVersion.V500));

        // 3.1.1 is the mostly used version so we preselect it.
        SelectedProtocolVersion = ProtocolVersions[1];
    }

    public ObservableCollection<ProtocolVersionViewModel> ProtocolVersions { get; } = new();

    public ProtocolVersionViewModel? SelectedProtocolVersion
    {
        get => _selectedProtocolVersion;
        set => this.RaiseAndSetIfChanged(ref _selectedProtocolVersion, value);
    }
}