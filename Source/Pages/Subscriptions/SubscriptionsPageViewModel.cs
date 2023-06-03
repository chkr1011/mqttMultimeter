using System;
using System.Linq;
using System.Threading.Tasks;
using mqttMultimeter.Common;
using mqttMultimeter.Pages.Subscriptions.State;
using mqttMultimeter.Services.Mqtt;
using mqttMultimeter.Services.State;

namespace mqttMultimeter.Pages.Subscriptions;

public sealed class SubscriptionsPageViewModel : BasePageViewModel
{
    readonly MqttClientService _mqttClientService;

    public SubscriptionsPageViewModel(MqttClientService mqttClientService, StateService stateService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        if (stateService == null)
        {
            throw new ArgumentNullException(nameof(stateService));
        }

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

        newItem.UserProperties.AddEmptyItem();

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
        stateService.TryGet(SubscriptionsPageState.Key, out SubscriptionsPageState? state);
        SubscriptionsPageStateLoader.Apply(this, state);

        Items.SelectedItem = Items.Collection.FirstOrDefault();
    }

    void SaveState(object? sender, SavingStateEventArgs eventArgs)
    {
        var state = SubscriptionsPageStateFactory.Create(this);
        eventArgs.StateService.Set(SubscriptionsPageState.Key, state);
    }
}