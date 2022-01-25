using System;
using System.Threading.Tasks;
using MQTTnet.App.Common;
using MQTTnet.App.Controls;
using ReactiveUI;

namespace MQTTnet.App.Pages.Publish;

public sealed class PublishItemViewModel : BaseViewModel
{
    string _contentType = string.Empty;

    uint _messageExpiryInterval;

    string _name = string.Empty;

    string _payload = string.Empty;

    string _responseTopic = string.Empty;

    bool _retain;

    uint _subscriptionIdentifier;

    string _topic = string.Empty;

    ushort _topicAlias;

    public PublishItemViewModel(PublishPageViewModel owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));

        Payload = string.Empty;
        PayloadFormatIndicator.IsUnspecified = true;

        Response.UserProperties.IsReadOnly = true;
    }

    public event Func<PublishItemViewModel, Task>? PublishRequested;

    public string ContentType
    {
        get => _contentType;
        set => this.RaiseAndSetIfChanged(ref _contentType, value);
    }

    public uint MessageExpiryInterval
    {
        get => _messageExpiryInterval;
        set => this.RaiseAndSetIfChanged(ref _messageExpiryInterval, value);
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public PublishPageViewModel Owner { get; }

    public string Payload
    {
        get => _payload;
        set => this.RaiseAndSetIfChanged(ref _payload, value);
    }

    public PayloadFormatIndicatorViewModel PayloadFormatIndicator { get; } = new();

    public QualityOfServiceLevelSelectorViewModel QualityOfServiceLevel { get; } = new();

    public PublishResponseViewModel Response { get; } = new();

    public string ResponseTopic
    {
        get => _responseTopic;
        set => this.RaiseAndSetIfChanged(ref _responseTopic, value);
    }

    public bool Retain
    {
        get => _retain;
        set => this.RaiseAndSetIfChanged(ref _retain, value);
    }

    public uint SubscriptionIdentifier
    {
        get => _subscriptionIdentifier;
        set => this.RaiseAndSetIfChanged(ref _subscriptionIdentifier, value);
    }

    public string Topic
    {
        get => _topic;
        set => this.RaiseAndSetIfChanged(ref _topic, value);
    }

    public ushort TopicAlias
    {
        get => _topicAlias;
        set => this.RaiseAndSetIfChanged(ref _topicAlias, value);
    }

    public UserPropertiesViewModel UserProperties { get; } = new();

    public Task Publish()
    {
        return PublishRequested?.Invoke(this) ?? Task.CompletedTask;
    }
}