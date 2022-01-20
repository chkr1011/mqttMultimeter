using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Subscriptions;

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

    public ReceivedApplicationMessageDetailsViewModel Details { get; }

    public int Id { get; }

    public bool IsRetained { get; }

    public int PayloadSize { get; }

    public string Topic { get; }
}