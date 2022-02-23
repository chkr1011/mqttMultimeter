using System;
using System.Linq;
using System.Threading.Tasks;
using MQTTnetApp.Common;
using MQTTnetApp.Pages.Subscriptions.State;
using MQTTnetApp.Services.Mqtt;
using MQTTnetApp.Services.State;

namespace MQTTnetApp.Pages.Subscriptions;

public sealed class SubscriptionsPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    public SubscriptionsPageViewModel(MqttClientService mqttClientService, StateService stateService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        // Make sure that we start with at least one item.
        AddItem();
        Items.SelectedItem = Items.Collection.FirstOrDefault();

        stateService.Saving += SaveState;
        LoadState(stateService);
    }

    public PageItemsViewModel<SubscriptionItemViewModel> Items { get; } = new();

    public void AddItem()
    {
        var newItem = new SubscriptionItemViewModel(this)
        {
            Name = "Untitled"
        };

        newItem.UserProperties.AddItem();

        Items.Collection.Add(newItem);
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

    void LoadState(StateService stateService)
    {
    }

    void SaveState(object? sender, SavingStateEventArgs eventArgs)
    {
        var state = SubscriptionsPageStateFactory.Create(this);
        eventArgs.StateService.Set("Subscriptions", state);
    }
}