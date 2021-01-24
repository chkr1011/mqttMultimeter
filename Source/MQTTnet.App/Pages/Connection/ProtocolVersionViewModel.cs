using MQTTnet.Formatter;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class ProtocolVersionViewModel
    {
        public ProtocolVersionViewModel(string displayName, MqttProtocolVersion protocolVersion)
        {
            DisplayName = displayName;
            Value = protocolVersion;
        }

        public string DisplayName { get; }

        public MqttProtocolVersion Value { get; }
    }
}