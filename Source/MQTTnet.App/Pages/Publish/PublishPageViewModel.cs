using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MQTTnet.App.Client.Service;
using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Publish;

public sealed class PublishPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

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
        get => GetValue<PublishItemViewModel>();
        set => SetValue(value);
    }

    public void AddItem()
    {
        var newItem = new PublishItemViewModel
        {
            Name = "Untitled"
        };

        newItem.PublishRequested += OnItemPublishRequested;
        newItem.DeleteRequested += OnItemDeleteRequested;

        Items.Add(newItem);
        SelectedItem = newItem;
    }

    public void ClearItems()
    {
        Items.Clear();
        SelectedItem = null;
    }

    void OnItemDeleteRequested(object? sender, EventArgs e)
    {
        Items.Remove((PublishItemViewModel) sender);
    }

    async Task OnItemPublishRequested(PublishItemViewModel arg)
    {
        try
        {
            var response = await _mqttClientService.Publish(arg);
            arg.Response.ApplyResponse(response);
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }
}