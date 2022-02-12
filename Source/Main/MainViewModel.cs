using MQTTnetApp.Common;
using MQTTnetApp.Pages.Connection;
using MQTTnetApp.Pages.Inflight;
using MQTTnetApp.Pages.Info;
using MQTTnetApp.Pages.Log;
using MQTTnetApp.Pages.PacketInspector;
using MQTTnetApp.Pages.Publish;
using MQTTnetApp.Pages.Subscriptions;

namespace MQTTnetApp.Main;

public sealed class MainViewModel : BaseViewModel
{
    public MainViewModel(ConnectionPageViewModel connectionPage,
        PublishPageViewModel publishPage,
        SubscriptionsPageViewModel subscriptionsPage,
        InflightPageViewModel inflightPage,
        PacketInspectorPageViewModel packetInspectorPage,
        InfoPageViewModel infoPage,
        LogPageViewModel logPage)
    {
        ConnectionPage = connectionPage;
        PublishPage = publishPage;
        SubscriptionsPage = subscriptionsPage;
        InflightPage = inflightPage;
        PacketInspectorPage = packetInspectorPage;
        InfoPage = infoPage;
        LogPage = logPage;
    }

    public ConnectionPageViewModel ConnectionPage { get; }

    public InflightPageViewModel InflightPage { get; }

    public InfoPageViewModel InfoPage { get; }

    public PacketInspectorPageViewModel PacketInspectorPage { get; }

    public PublishPageViewModel PublishPage { get; }
    
    public LogPageViewModel LogPage { get; }

    public SubscriptionsPageViewModel SubscriptionsPage { get; }
}