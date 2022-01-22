using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using MQTTnet.App.Client.Service;
using MQTTnet.App.Common;
using MQTTnet.Client;

namespace MQTTnet.App.Pages.Subscriptions;

public sealed class SubscriptionsPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    int _messageId;


    public SubscriptionsPageViewModel(MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        mqttClientService.ApplicationMessageReceived += HandleApplicationMessageReceivedAsync;
    }

    public ViewModelCollection<ReceivedApplicationMessageViewModel> ReceivedApplicationMessages { get; } = new();

    public ViewModelCollection<SubscriptionViewModel> Subscriptions { get; } = new();

    public void Clear()
    {
        ReceivedApplicationMessages.Clear();
    }

    public async Task CreateSubscription()
    {
        try
        {
            var editor = new SubscriptionEditorViewModel(_mqttClientService);

            var window = new SubscriptionEditorView
            {
                Title = "Create subscription",
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ShowActivated = true,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInTaskbar = false,
                CanResize = false,
                DataContext = editor
            };

            await App.ShowDialog(window);

            if (!editor.Subscribed)
            {
                return;
            }

            var subscription = new SubscriptionViewModel(editor.ConfigurationPage);

            subscription.UnsubscribedHandler = async () =>
            {
                await _mqttClientService.Unsubscribe(editor.ConfigurationPage.Topic);
                Subscriptions.Remove(subscription);
            };

            Subscriptions.Add(subscription);
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }

    Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        return Dispatcher.UIThread.InvokeAsync(() =>
        {
            ReceivedApplicationMessages.Add(new ReceivedApplicationMessageViewModel(_messageId++, eventArgs.ApplicationMessage));
        });
    }
}