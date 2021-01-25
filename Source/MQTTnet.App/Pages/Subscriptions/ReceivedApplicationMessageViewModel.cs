using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class ReceivedApplicationMessageViewModel : BaseViewModel
    {
        public ReceivedApplicationMessageViewModel(int id, MqttApplicationMessage message)
        {
            Id = id;
            Topic = message.Topic;
            IsRetained = message.Retain;
            PayloadSize = message.Payload?.Length ?? 0;

            Details = new ReceivedApplicationMessageDetailsViewModel(message);
        }

        public int PayloadSize { get; }

        public int Id { get; }

        public string Topic { get; }

        public bool IsRetained { get; }

        public ReceivedApplicationMessageDetailsViewModel Details { get; }
    }
}