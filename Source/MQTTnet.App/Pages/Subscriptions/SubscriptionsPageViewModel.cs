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

        newItem.UserProperties.AddItem();

        Items.Add(newItem);
    }

    public void ClearItems()
    {
        Items.Clear();
    }

    public void RemoveItem(SubscriptionItemViewModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        Items.Remove(item);
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
}