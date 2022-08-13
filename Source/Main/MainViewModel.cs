using MQTTnetApp.Common;
using MQTTnetApp.Pages.Connection;
using MQTTnetApp.Pages.Inflight;
using MQTTnetApp.Pages.Info;
using MQTTnetApp.Pages.Log;
using MQTTnetApp.Pages.PacketInspector;
using MQTTnetApp.Pages.Publish;
using MQTTnetApp.Pages.Subscriptions;
using MQTTnetApp.Pages.TopicExplorer;
using ReactiveUI;

namespace MQTTnetApp.Main;

public sealed class MainViewModel : BaseViewModel
{
    object? _overlayContent;

    public MainViewModel(ConnectionPageViewModel connectionPage,
        PublishPageViewModel publishPage,
        SubscriptionsPageViewModel subscriptionsPage,
        InflightPageViewModel inflightPage,
        TopicExplorerPageViewModel topicExplorerPage,
        PacketInspectorPageViewModel packetInspectorPage,
        InfoPageViewModel infoPage,
        LogPageViewModel logPage)
    {
        ConnectionPage = connectionPage;
        PublishPage = publishPage;
        SubscriptionsPage = subscriptionsPage;
        InflightPage = inflightPage;
        TopicExplorerPage = topicExplorerPage;
        PacketInspectorPage = packetInspectorPage;
        InfoPage = infoPage;
        LogPage = logPage;
    }

    public ConnectionPageViewModel ConnectionPage { get; }

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
}