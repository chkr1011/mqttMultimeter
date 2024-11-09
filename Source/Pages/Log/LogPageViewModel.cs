using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using mqttMultimeter.Common;
using mqttMultimeter.Services.Mqtt;
using MQTTnet.Diagnostics.Logger;
using ReactiveUI;

namespace mqttMultimeter.Pages.Log;

public sealed class LogPageViewModel : BasePageViewModel
{
    bool _isRecordingEnabled;

    public LogPageViewModel(MqttClientService mqttClientService)
    {
        var mqttClientService1 = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        mqttClientService1.LogMessagePublished += MqttClientServiceOnLogMessagePublished;
    }

    public bool IsRecordingEnabled
    {
        get => _isRecordingEnabled;
        set => this.RaiseAndSetIfChanged(ref _isRecordingEnabled, value);
    }

    public ObservableCollection<LogItemViewModel> Items { get; } = new();

    public void ClearItems()
    {
        Items.Clear();
    }

    void MqttClientServiceOnLogMessagePublished(MqttNetLogMessagePublishedEventArgs eventArgs)
    {
        if (!_isRecordingEnabled)
        {
            return;
        }

        var newItem = new LogItemViewModel
        {
            IsVerbose = eventArgs.LogMessage.Level == MqttNetLogLevel.Verbose,
            IsInformation = eventArgs.LogMessage.Level == MqttNetLogLevel.Info,
            IsWarning = eventArgs.LogMessage.Level == MqttNetLogLevel.Warning,
            IsError = eventArgs.LogMessage.Level == MqttNetLogLevel.Error,
            Timestamp = eventArgs.LogMessage.Timestamp.ToString("HH:mm:ss.fff"),
            Source = eventArgs.LogMessage.Source,
            Message = eventArgs.LogMessage.Message,
            Level = eventArgs.LogMessage.Level.ToString(),
            Exception = eventArgs.LogMessage.Exception?.ToString()
        };

        Dispatcher.UIThread.Post(() =>
        {
            Items.Add(newItem);
        });
    }
}