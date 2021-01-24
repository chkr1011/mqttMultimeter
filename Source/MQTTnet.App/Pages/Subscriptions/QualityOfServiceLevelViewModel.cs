using MQTTnet.App.Common;
using MQTTnet.Protocol;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class QualityOfServiceLevelViewModel : BaseViewModel
    {
        public QualityOfServiceLevelViewModel(string displayName, MqttQualityOfServiceLevel value)
        {
            DisplayName = displayName;
            Value = value;
        }

        public string DisplayName { get; }

        public MqttQualityOfServiceLevel Value { get; }
    }
}