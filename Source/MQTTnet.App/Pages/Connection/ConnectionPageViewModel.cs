using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Mqtt;
using ReactiveUI;

namespace MQTTnet.App.Pages.Connection;

public sealed class ConnectionPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    bool _isConnected;

    bool _isConnecting;

    public ConnectionPageViewModel(MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService;

        var timer = new DispatcherTimer(TimeSpan.FromSeconds(0.5), DispatcherPriority.Normal, CheckConnection);
        timer.Start();
    }

    public bool IsConnected
    {
        get => _isConnected;
        set => this.RaiseAndSetIfChanged(ref _isConnected, value);
    }

    public bool IsConnecting
    {
        get => _isConnecting;
        private set => this.RaiseAndSetIfChanged(ref _isConnecting, value);
    }

    public MqttNetOptionsViewModel MqttNetOptions { get; } = new();

    public ProtocolOptionsViewModel ProtocolOptions { get; } = new();

    public ConnectResponseViewModel Response { get; } = new();

    public ServerOptionsViewModel ServerOptions { get; } = new();

    public SessionOptionsViewModel SessionOptions { get; } = new();

    public async Task Connect()
    {
        try
        {
            IsConnecting = true;

            var response = await _mqttClientService.Connect(this);
            Response.ApplyResponse(response);
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

    void CheckConnection(object? sender, EventArgs e)
    {
        IsConnected = _mqttClientService.IsConnected;
    }
}