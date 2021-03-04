using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class MqttNetOptionsViewModel : BaseViewModel
    {
        public bool EnablePacketInspection
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool EnableLogging
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }
    }
}
