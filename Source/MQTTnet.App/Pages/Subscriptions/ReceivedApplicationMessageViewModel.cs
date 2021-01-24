using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class ReceivedApplicationMessageViewModel : BaseViewModel
    {
        public ReceivedApplicationMessageViewModel(MqttApplicationMessage message)
        {
            Topic = message.Topic;

            Details = new ReceivedApplicationMessageDetailsViewModel(message);
        }

        public string Topic { get; }

        public ReceivedApplicationMessageDetailsViewModel Details { get; }
    }
}