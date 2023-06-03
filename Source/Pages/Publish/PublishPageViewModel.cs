using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mqttMultimeter.Common;
using mqttMultimeter.Pages.Inflight;
using mqttMultimeter.Pages.Publish.State;
using mqttMultimeter.Services.Mqtt;
using mqttMultimeter.Services.State;

namespace mqttMultimeter.Pages.Publish;

public sealed class PublishPageViewModel : BasePageViewModel
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
        newItem.UserProperties.AddEmptyItem();

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

    public void RepeatMessage(InflightPageItemViewModel inflightPageItem)
    {
        if (inflightPageItem == null)
        {
            throw new ArgumentNullException(nameof(inflightPageItem));
        }

        var publishItem = new PublishItemViewModel(this)
        {
            Name = $"Repeat '{inflightPageItem.Topic}'",
            ContentType = inflightPageItem.ContentType,
            Topic = inflightPageItem.Topic,
            Payload = Encoding.UTF8.GetString(inflightPageItem.Payload)
        };

        Items.Collection.Add(publishItem);
        Items.SelectedItem = publishItem;

        RequestActivation();
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