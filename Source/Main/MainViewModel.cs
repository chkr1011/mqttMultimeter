using System;
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
    }

    public event EventHandler? ActivatePageRequested;

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

    TPage AttachEvents<TPage>(TPage page) where TPage : BasePageViewModel
    {
        page.ActivationRequested += (_, __) => ActivatePageRequested?.Invoke(page, EventArgs.Empty);
        return page;
    }
}