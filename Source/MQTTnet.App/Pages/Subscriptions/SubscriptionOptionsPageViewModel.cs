using MQTTnet.App.Common;
using MQTTnet.App.Common.QualityOfServiceLevel;
using MQTTnet.Protocol;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class SubscriptionOptionsPageViewModel : BaseViewModel
    {
        public string Topic { get; set; } = string.Empty;

        public QualityOfServiceLevelSelectorViewModel QualityOfServiceLevel { get; } = new QualityOfServiceLevelSelectorViewModel();

        public bool NoLocal { get; set; }

        public bool RetainAsPublished { get; set; }

        public bool IsRetainHandling0 { get; set; } = true;

        public bool IsRetainHandling1 { get; set; }

        public MqttRetainHandling RetainHandling
        {
            // TODO: This should be refactored as soon as Avalonia supports Binding.DoNothing!
            get
            {
                if (IsRetainHandling0)
                {
                    return MqttRetainHandling.SendAtSubscribe;
                }

                if (IsRetainHandling1)
                {
                    return MqttRetainHandling.SendAtSubscribeIfNewSubscriptionOnly;
                }

                return MqttRetainHandling.SendAtSubscribe;
            }
        }

        public bool Validate()
        {
            ClearErrors();

            if (string.IsNullOrEmpty(Topic))
            {
                SetErrors(nameof(Topic), "Value must not be empty.");
            }

            return !HasErrors;
        }
    }
}