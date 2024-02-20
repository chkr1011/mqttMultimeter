using System;
using Avalonia.Threading;
using mqttMultimeter.Common;
using mqttMultimeter.Pages.Connection;
using mqttMultimeter.Pages.Inflight;
using mqttMultimeter.Pages.Info;
using mqttMultimeter.Pages.Log;
using mqttMultimeter.Pages.PacketInspector;
using mqttMultimeter.Pages.Publish;
using mqttMultimeter.Pages.Subscriptions;
using mqttMultimeter.Pages.TopicExplorer;
using mqttMultimeter.Services.Mqtt;
using ReactiveUI;

namespace mqttMultimeter.Main;

public sealed class MainViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    int _counter;
    object? _overlayContent;

    public MainViewModel(ConnectionPageViewModel connectionPage,
        PublishPageViewModel publishPage,
        SubscriptionsPageViewModel subscriptionsPage,
        InflightPageViewModel inflightPage,
        TopicExplorerPageViewModel topicExplorerPage,
        PacketInspectorPageViewModel packetInspectorPage,
        InfoPageViewModel infoPage,
        LogPageViewModel logPage,
        MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        ConnectionPage = AttachEvents(connectionPage);
        PublishPage = AttachEvents(publishPage);
        SubscriptionsPage = AttachEvents(subscriptionsPage);
        InflightPage = AttachEvents(inflightPage);
        TopicExplorerPage = AttachEvents(topicExplorerPage);
        PacketInspectorPage = AttachEvents(packetInspectorPage);
        InfoPage = AttachEvents(infoPage);
        LogPage = AttachEvents(logPage);

        InflightPage.RepeatMessageRequested += item => PublishPage.RepeatMessage(item);
        topicExplorerPage.RepeatMessageRequested += item => PublishPage.RepeatMessage(item);

        // Update the counter with a timer. There is no need to trigger a binding
        // for each counter increment.
        DispatcherTimer.Run(UpdateCounter, TimeSpan.FromSeconds(1));
    }

    public event EventHandler? ActivatePageRequested;

    public ConnectionPageViewModel ConnectionPage { get; }

    public int Counter
    {
        get => _counter;
        set => this.RaiseAndSetIfChanged(ref _counter, value);
    }

    public InflightPageViewModel InflightPage { get; }

    public InfoPageViewModel InfoPage { get; }

    public LogPageViewModel LogPage { get; }

    public object? OverlayContent
    {
        get => _overlayContent;
        set => this.RaiseAndSetIfChanged(ref _overlayContent, value);
    }

    public PacketInspectorPageViewModel PacketInspectorPage { get; }

    public PublishPageViewModel PublishPage { get; }

    public SubscriptionsPageViewModel SubscriptionsPage { get; }

    public TopicExplorerPageViewModel TopicExplorerPage { get; }

    TPage AttachEvents<TPage>(TPage page) where TPage : BasePageViewModel
    {
        page.ActivationRequested += (_, __) => ActivatePageRequested?.Invoke(page, EventArgs.Empty);
        return page;
    }

    bool UpdateCounter()
    {
        Counter = _mqttClientService.ReceivedMessagesCount;
        return true;
    }
}