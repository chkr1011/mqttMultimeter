using System;
using System.Threading.Tasks;
using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class SubscriptionViewModel : BaseViewModel
    {
        public SubscriptionViewModel(SubscriptionEditorViewModel options)
        {
            Options = options;
        }

        public Func<Task> UnsubscribedHandler { get; set; }

        public SubscriptionEditorViewModel Options { get; }

        public Task Unsubscribe()
        {
            return UnsubscribedHandler.Invoke();
        }
    }
}