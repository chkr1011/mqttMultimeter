using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Client;

namespace MQTTnet.App.Pages.Publish;

public sealed class PublishPageViewModel : BasePageViewModel
{
    readonly MqttClientService _mqttClientService;

    public PublishPageViewModel(MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        Header = new TextViewModel("Publish");

        // Make sure that we start with at least one item.
        AddItem();
    }

    public ObservableCollection<PublishItemViewModel> Items { get; } = new();

    public void AddItem()
    {
        var newItem = new PublishItemViewModel
        {
            Name = "Untitled"
        };

        newItem.PublishRequested += OnItemPublishRequested;
        
        Items.Add(newItem);
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