using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using MQTTnet.Adapter;
using MQTTnet.App.Client.Service;
using MQTTnet.App.Common;
using MQTTnet.App.Common.ObjectDump;

namespace MQTTnet.App.Pages.Connection;

public sealed class ConnectionPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    public ConnectionPageViewModel(MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService;
        
        var timer = new DispatcherTimer(TimeSpan.FromSeconds(0.5), DispatcherPriority.Normal, TimerCallback);
        timer.Start();
    }

    public ConnectionPageHeaderViewModel Header { get; } = new(); 

    public bool IsConnecting
    {
        get => GetValue<bool>();
        private set => SetValue(value);
    }

    public MqttNetOptionsViewModel MqttNetOptions { get; } = new();

    public ProtocolOptionsViewModel ProtocolOptions { get; } = new();

    public ObjectDumpViewModel Result { get; } = new();

    public ServerOptionsViewModel ServerOptions { get; } = new();

    public SessionOptionsViewModel SessionOptions { get; } = new();

    public async Task Connect()
    {
        try
        {
            IsConnecting = true;

            var result = await _mqttClientService.Connect(this);

            Result.Dump(result);
        }
        catch (MqttConnectingFailedException exception)
        {
            Result.Dump(exception.Result);
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
        finally
        {
            IsConnecting = false;
        }
    }

    public async Task Disconnect()
    {
        try
        {
            await _mqttClientService.Disconnect();
        }
        catch (Exception exception)
        {
            App.ShowException(exception);
        }
    }

    void TimerCallback(object? sender, EventArgs e)
    {
        Header.IsConnected = _mqttClientService.IsConnected;
    }
}