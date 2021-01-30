using System;
using MQTTnet.App.Common;
using MQTTnet.App.Common.QualityOfServiceLevel;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class SubscriptionEditorViewModel : BaseViewModel
    {
        public event EventHandler Completed;

        public string Topic
        {
            get;
            set;
        }

        public QualityOfServiceLevelSelectorViewModel QualityOfServiceLevel { get; } = new QualityOfServiceLevelSelectorViewModel();

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