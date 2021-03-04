using MQTTnet.App.Common;
using MQTTnet.Formatter;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class ProtocolOptionsViewModel : BaseViewModel
    {
        public ProtocolOptionsViewModel()
        {
            ProtocolVersions.Add(new ProtocolVersionViewModel("3.1.0", MqttProtocolVersion.V310));
            ProtocolVersions.Add(new ProtocolVersionViewModel("3.1.1", MqttProtocolVersion.V311));
            ProtocolVersions.Add(new ProtocolVersionViewModel("5.0.0", MqttProtocolVersion.V500));

            // 3.1.1 is the mostly used version so we preselect it.
            ProtocolVersions.SelectedItem = ProtocolVersions[1];
        }

        public ViewModelCollection<ProtocolVersionViewModel> ProtocolVersions { get; } = new();
    }
}