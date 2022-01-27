using System.Reflection;
using MQTTnet.App.Common;
using MQTTnet.App.Pages.Connection;
using MQTTnet.App.Pages.Inflight;
using MQTTnet.App.Pages.Info;
using MQTTnet.App.Pages.PacketInspector;
using MQTTnet.App.Pages.Publish;
using MQTTnet.App.Pages.Subscriptions;

namespace MQTTnet.App.Main;

public sealed class MainViewModel : BaseViewModel
{
    public MainViewModel(ConnectionPageViewModel connectionPage,
        PublishPageViewModel publishPage,
        SubscriptionsPageViewModel subscriptionsPage,
        InflightPageViewModel inflightPage,
        PacketInspectorPageViewModel packetInspectorPage,
        InfoPageViewModel infoPage)
    {
        ConnectionPage = connectionPage;
        PublishPage = publishPage;
        SubscriptionsPage = subscriptionsPage;
        InflightPage = inflightPage;
        PacketInspectorPage = packetInspectorPage;
        InfoPage = infoPage;

        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
    }

    public ConnectionPageViewModel ConnectionPage { get; }

    public InflightPageViewModel InflightPage { get; }

    public InfoPageViewModel InfoPage { get; }

    public PacketInspectorPageViewModel PacketInspectorPage { get; }

    public PublishPageViewModel PublishPage { get; }

    public SubscriptionsPageViewModel SubscriptionsPage { get; }

    public string Version { get; }
}