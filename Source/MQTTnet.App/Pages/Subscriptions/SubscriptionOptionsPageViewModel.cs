using MQTTnet.App.Common;
using MQTTnet.App.Controls.QualityOfServiceLevel;
using MQTTnet.Protocol;

namespace MQTTnet.App.Pages.Subscriptions;

public sealed class SubscriptionOptionsPageViewModel : BaseViewModel
{
    public bool IsRetainHandling0 { get; set; } = true;

    public bool IsRetainHandling1 { get; set; }

    public bool NoLocal { get; set; }

    public QualityOfServiceLevelSelectorViewModel QualityOfServiceLevel { get; } = new();

    public bool RetainAsPublished { get; set; }

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

    public string Topic { get; set; } = string.Empty;

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