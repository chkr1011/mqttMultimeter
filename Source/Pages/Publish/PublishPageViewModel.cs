using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MQTTnetApp.Common;
using MQTTnetApp.Services.Mqtt;
using ReactiveUI;

namespace MQTTnetApp.Pages.Publish;

public sealed class PublishPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    PublishItemViewModel? _selectedItem;

    public PublishPageViewModel(MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        // Make sure that we start with at least one item.
        AddItem();
        SelectedItem = Items.FirstOrDefault();
    }

    public ObservableCollection<PublishItemViewModel> Items { get; } = new();

    public PublishItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public void AddItem()
    {
        var newItem = new PublishItemViewModel(this)
        {
            Name = "Untitled"
        };

        // Prepare the UI with at lest one user property.
        // It will not be send when the name is empty.
        newItem.UserProperties.AddItem();

        Items.Add(newItem);
        SelectedItem = newItem;
    }

    public void ClearItems()
    {
        Items.Clear();
        SelectedItem = null;
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

        Items.Remove(item);
    }
}