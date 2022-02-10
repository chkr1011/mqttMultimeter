using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using MQTTnet.Diagnostics;
using MQTTnetApp.Common;
using MQTTnetApp.Services.Mqtt;

namespace MQTTnetApp.Pages.Log;

public sealed class LogPageViewModel : BaseViewModel
{
    readonly MqttClientService? _mqttClientService;

    public LogPageViewModel(MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        _mqttClientService.LogMessagePublished += MqttClientServiceOnLogMessagePublished;
    }

    public ObservableCollection<LogItemViewModel> Items { get; } = new();

    public void ClearItems()
    {
        Items.Clear();
    }
    
    void MqttClientServiceOnLogMessagePublished(MqttNetLogMessagePublishedEventArgs obj)
    {
        var newItem = new LogItemViewModel
        {
            Timestamp = obj.LogMessage.Timestamp.ToString("HH:mm:ss.fff"),
            Level = obj.LogMessage.Level.ToString(),
            Source = obj.LogMessage.Source,
            Message = obj.LogMessage.Message
        };
        
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Items.Add(newItem);
        });
    }
}