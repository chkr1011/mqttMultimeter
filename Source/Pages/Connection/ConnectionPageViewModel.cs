using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using MQTTnetApp.Common;
using MQTTnetApp.Services.Mqtt;
using ReactiveUI;

namespace MQTTnetApp.Pages.Connection;

public sealed class ConnectionPageViewModel : BaseViewModel
{
    readonly MqttClientService _mqttClientService;

    bool _isConnected;
    bool _isConnecting;

    ConnectionItemViewModel? _selectedItem;

    public ConnectionPageViewModel(MqttClientService mqttClientService)
    {
        _mqttClientService = mqttClientService;

        var timer = new DispatcherTimer(TimeSpan.FromSeconds(0.5), DispatcherPriority.Normal, CheckConnection);
        timer.Start();

        Items.Add(new ConnectionItemViewModel(this)
        {
            Name = "localhost",
            ServerOptions =
            {
                Host = "localhost"
            }
        });

        Items.Add(new ConnectionItemViewModel(this)
        {
            Name = "Hive MQ",
            ServerOptions =
            {
                Host = "broker.hivemq.com"
            }
        });

        Items.Add(new ConnectionItemViewModel(this)
        {
            Name = "Mosquitto Test",
            ServerOptions =
            {
                Host = "test.mosquitto.org"
            }
        });

        SelectedItem = Items[0];
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

    public ObservableCollection<ConnectionItemViewModel> Items { get; } = new();

    public ConnectionItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

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

        Items.Add(newItem);
        SelectedItem = newItem;
    }

    public void ClearItems()
    {
        Items.Clear();
        SelectedItem = null;
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

    public void RemoveItem(ConnectionItemViewModel item)
    {
        Items.Remove(item);
    }

    void CheckConnection(object? sender, EventArgs e)
    {
        IsConnected = _mqttClientService.IsConnected;
    }
}