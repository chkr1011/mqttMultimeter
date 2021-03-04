using System;
using System.Threading.Tasks;
using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class SubscriptionViewModel : BaseViewModel
    {
        public SubscriptionViewModel(SubscriptionOptionsPageViewModel configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Func<Task> UnsubscribedHandler { get; set; }

        public SubscriptionOptionsPageViewModel Configuration { get; }

        public Task Unsubscribe()
        {
            return UnsubscribedHandler.Invoke();
        }
    }
}