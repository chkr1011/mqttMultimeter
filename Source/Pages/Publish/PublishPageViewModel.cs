using System;
using System.Linq;
using System.Threading.Tasks;
using MQTTnetApp.Common;
using MQTTnetApp.Pages.Publish.State;
using MQTTnetApp.Services.Mqtt;
using MQTTnetApp.Services.State;

namespace MQTTnetApp.Pages.Publish;

public sealed class PublishPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    public PublishPageViewModel(MqttClientService mqttClientService, StateService stateService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        if (stateService == null)
        {
            throw new ArgumentNullException(nameof(stateService));
        }

        stateService.Saving += SaveState;
        LoadState(stateService);
    }

    public PageItemsViewModel<PublishItemViewModel> Items { get; } = new();

    public void AddItem()
    {
        var newItem = new PublishItemViewModel(this)
        {
            Name = "Untitled"
        };

        // Prepare the UI with at lest one user property.
        // It will not be send when the name is empty.
        newItem.UserProperties.AddItem();

        Items.Collection.Add(newItem);
        Items.SelectedItem = newItem;
    }

    public async Task PublishItem(PublishItemViewModel item)
    {
        try
        {
            var response = await _mqttClientService.Publish(item);
            item.Response.ApplyResponse(response);
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }

    public void RemoveItem(PublishItemViewModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        Items.RemoveItem(item);
    }

    void LoadState(StateService stateService)
    {
        stateService.TryGet(PublishPageState.Key, out PublishPageState? state);
        PublishPageStateLoader.Apply(this, state);

        Items.SelectedItem = Items.Collection.FirstOrDefault();
    }

    void SaveState(object? sender, SavingStateEventArgs eventArgs)
    {
        var state = PublishPageStateFactory.Create(this);
        eventArgs.StateService.Set(PublishPageState.Key, state);
    }
}