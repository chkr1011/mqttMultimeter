using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using MQTTnetApp.Common;
using MQTTnetApp.Pages.Connection.State;
using MQTTnetApp.Services.Mqtt;
using MQTTnetApp.Services.State;
using ReactiveUI;

namespace MQTTnetApp.Pages.Connection;

public sealed class ConnectionPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    bool _isConnected;
    bool _isConnecting;

    public ConnectionPageViewModel(MqttClientService mqttClientService, StateService stateService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));

        if (stateService == null)
        {
            throw new ArgumentNullException(nameof(stateService));
        }

        var timer = new DispatcherTimer(TimeSpan.FromSeconds(0.1), DispatcherPriority.Normal, CheckConnection);
        timer.Start();

        stateService.Saving += SaveState;
        LoadState(stateService);
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

    public PageItemsViewModel<ConnectionItemViewModel> Items { get; } = new();

    public void AddItem()
    {
        var newItem = new ConnectionItemViewModel(this)
        {
            Name = "Untitled",
            ServerOptions =
            {
                Host = "localhost"
            }
        };

        Items.Collection.Add(newItem);
        Items.SelectedItem = newItem;
    }

    public async Task Connect(ConnectionItemViewModel item)
    {
        try
        {
            IsConnecting = true;

            var response = await _mqttClientService.Connect(item);
            item.Response.ApplyResponse(response);
        }
        catch (Exception exception)
        {
            // Ensure proper UI state before showing the exception.
            IsConnecting = false;

            App.ShowException(exception);
        }
        finally
        {
            IsConnecting = false;
        }
    }

    public async Task Disconnect(ConnectionItemViewModel item)
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

    void LoadState(StateService stateService)
    {
        stateService.TryGet(ConnectionPageState.Key, out ConnectionPageState? state);
        ConnectionPageStateLoader.Apply(this, state);

        Items.SelectedItem = Items.Collection.FirstOrDefault();
    }

    void SaveState(object? sender, SavingStateEventArgs eventArgs)
    {
        var state = ConnectionPageStateFactory.Create(this);
        eventArgs.StateService.Set(ConnectionPageState.Key, state);
    }
}