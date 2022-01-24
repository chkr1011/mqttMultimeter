using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Mqtt;
using MQTTnet.Client;

namespace MQTTnet.App.Pages.Inflight;

public sealed class InflightPageViewModel : BaseViewModel
{
    int _number;

    public InflightPageViewModel(MqttClientService mqttClientService)
    {
        mqttClientService.ApplicationMessageReceived += OnApplicationMessageReceived;
    }

    public ObservableCollection<ReceivedApplicationMessageViewModel> Items { get; } = new();

    public ReceivedApplicationMessageViewModel SelectedItem
    {
        get => GetValue<ReceivedApplicationMessageViewModel>();
        set => SetValue(value);
    }
    
    public void ClearItems()
    {
        Items.Clear();
    }
    
    ReceivedApplicationMessageViewModel CreateViewModel(MqttApplicationMessage applicationMessage)
    {
        return new ReceivedApplicationMessageViewModel
        {
            Timestamp = DateTime.Now,
            Number = _number++,
            Topic = applicationMessage.Topic,
            Length = applicationMessage.Payload?.Length ?? 0L,
            Source = applicationMessage
        };
    }

    Task OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        return Dispatcher.UIThread.InvokeAsync(() =>
        {
            Items.Add(CreateViewModel(eventArgs.ApplicationMessage));
        });
    }
}