using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Mqtt;
using ReactiveUI;

namespace MQTTnet.App.Pages.Subscriptions;

public sealed class SubscriptionsPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    SubscriptionItemViewModel? _selectedItem;

    public SubscriptionsPageViewModel(MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        // Make sure that we start with at least one item.
        AddItem();
        SelectedItem = Items.FirstOrDefault();
    }

    public ObservableCollection<SubscriptionItemViewModel> Items { get; } = new();

    public SubscriptionItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public void AddItem()
    {
        var newItem = new SubscriptionItemViewModel(this)
        {
            Name = "Untitled",
            Topic = "#"
        };

        Items.Add(newItem);
    }

    public void ClearItems()
    {
        Items.Clear();
    }
    
    public async Task SubscribeItem(SubscriptionItemViewModel item)
    {
        try
        {
            var response = await _mqttClientService.Subscribe(item);
            item.Response.ApplyResponse(response);
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }
    
    public async Task UnsubscribeItem(SubscriptionItemViewModel item)
    {
        try
        {
            var response = await _mqttClientService.Unsubscribe(item);
            item.Response.ApplyResponse(response);
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }
    
    public async Task CreateSubscription()
    {
        try
        {
            //var editor = new SubscribeResponseViewModel(_mqttClientService);

            // var window = new SubscriptionEditorView
            // {
            //     Title = "Create subscription",
            //     WindowStartupLocation = WindowStartupLocation.CenterOwner,
            //     ShowActivated = true,
            //     SizeToContent = SizeToContent.WidthAndHeight,
            //     ShowInTaskbar = false,
            //     CanResize = false,
            //     DataContext = editor
            // };

            // await App.ShowDialog(window);
            //
            // if (!editor.Subscribed)
            // {
            //     return;
            // }

            // var subscription = new SubscriptionViewModel(editor.ConfigurationPage);
            //
            // subscription.UnsubscribedHandler = async () =>
            // {
            //     await _mqttClientService.Unsubscribe(editor.ConfigurationPage.Topic);
            //     Items.Remove(subscription);
            // };
            //
            // Items.Add(subscription);
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }
}