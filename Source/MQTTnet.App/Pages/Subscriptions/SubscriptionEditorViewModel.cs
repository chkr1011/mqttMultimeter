using System.Linq;
using MQTTnet.App.Common;
using MQTTnet.Protocol;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class SubscriptionEditorViewModel : BaseViewModel
    {
        public SubscriptionEditorViewModel()
        {
            QualityOfServiceLevels.Add(new QualityOfServiceLevelViewModel("0 - At most once", MqttQualityOfServiceLevel.AtMostOnce));
            QualityOfServiceLevels.Add(new QualityOfServiceLevelViewModel("1 - At least once", MqttQualityOfServiceLevel.AtLeastOnce));
            QualityOfServiceLevels.Add(new QualityOfServiceLevelViewModel("2 - Exactly once", MqttQualityOfServiceLevel.ExactlyOnce));
            QualityOfServiceLevels.SelectedItem = QualityOfServiceLevels.FirstOrDefault()!;
        }

        public string Topic
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public ViewModelCollection<QualityOfServiceLevelViewModel> QualityOfServiceLevels { get; } = new ViewModelCollection<QualityOfServiceLevelViewModel>();
    }
}