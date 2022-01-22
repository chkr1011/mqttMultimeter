using MQTTnet.App.Common;
using MQTTnet.App.Pages.Connection;
using MQTTnet.App.Pages.Info;
using MQTTnet.App.Pages.PacketInspector;
using MQTTnet.App.Pages.Publish;
using MQTTnet.App.Pages.Subscriptions;

namespace MQTTnet.App.Main;

public sealed class MainViewModel : BaseViewModel
{
    public MainViewModel(ConnectionPageViewModel connectionPage,
        SubscriptionsPageViewModel subscriptionsPage,
        PublishPageViewModel publishPage,
        PacketInspectorPageViewModel packetInspectorPage,
        InfoPageViewModel infoPage)
    {
        ConnectionPage = connectionPage;
        SubscriptionsPage = subscriptionsPage;
        PublishPage = publishPage;
        PacketInspectorPage = packetInspectorPage;
        InfoPage = infoPage;
    }

    public ConnectionPageViewModel ConnectionPage { get; }
    
    public SubscriptionsPageViewModel SubscriptionsPage { get; }
    
    public PublishPageViewModel PublishPage { get; }
    
    public PacketInspectorPageViewModel PacketInspectorPage { get; }
    
    public InfoPageViewModel InfoPage { get; }
}