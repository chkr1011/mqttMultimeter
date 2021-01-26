using System;
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

        public event EventHandler Completed;

        public string Topic
        {
            get;
            set;
        }

        public bool NoLocal
        {
            get;
            set;
        }

        public bool RetainAsPublished
        {
            get;
            set;
        }

        public ViewModelCollection<QualityOfServiceLevelViewModel> QualityOfServiceLevels { get; } = new ViewModelCollection<QualityOfServiceLevelViewModel>();

        public bool Accepted { get; private set; }

        public void Accept()
        {
            if (string.IsNullOrEmpty(Topic))
            {
                SetErrors(nameof(Topic), "Value must not be empty.");
                return;
            }

            Accepted = true;
            Completed.Invoke(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            Completed.Invoke(this, EventArgs.Empty);
        }
    }
}