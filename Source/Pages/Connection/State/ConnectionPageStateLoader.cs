using System;
using System.Linq;

namespace MQTTnetApp.Pages.Connection.State;

public static class ConnectionPageStateLoader
{
    public static void Apply(ConnectionPageViewModel target, ConnectionPageState? state)
    {
        if (target == null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        if (state == null)
        {
            AddDemoEntries(target);
            return;
        }

        foreach (var connectionState in state.Connections)
        {
            target.Items.Collection.Add(CreateItem(target, connectionState));
        }
    }

    static void AddDemoEntries(ConnectionPageViewModel target)
    {
        target.Items.Collection.Add(new ConnectionItemViewModel(target)
        {
            Name = "localhost",
            ServerOptions =
            {
                Host = "localhost"
            }
        });

        target.Items.Collection.Add(new ConnectionItemViewModel(target)
        {
            Name = "Hive MQ",
            ServerOptions =
            {
                Host = "broker.hivemq.com"
            }
        });

        target.Items.Collection.Add(new ConnectionItemViewModel(target)
        {
            Name = "Mosquitto Test",
            ServerOptions =
            {
                Host = "test.mosquitto.org"
            }
        });
    }

    static ConnectionItemViewModel CreateItem(ConnectionPageViewModel ownerPage, ConnectionState connectionState)
    {
        var item = new ConnectionItemViewModel(ownerPage)
        {
            Name = connectionState.Name ?? string.Empty,
            ServerOptions =
            {
                Host = connectionState.Host ?? string.Empty,
                Port = connectionState.Port
            },
            SessionOptions =
            {
                UserName = connectionState.UserName ?? string.Empty,
                KeepAliveInterval = connectionState.KeepAliveInterval,
                AuthenticationMethod = connectionState.AuthenticationMethod ?? string.Empty
            }
        };

        item.ServerOptions.SelectedTransport =
            item.ServerOptions.Transports.FirstOrDefault(t => t.Value.Equals(connectionState.Transport)) ?? item.ServerOptions.Transports.First();

        item.ServerOptions.SelectedProtocolVersion = item.ServerOptions.ProtocolVersions.FirstOrDefault(p => p.Value.Equals(connectionState.ProtocolVersion)) ??
                                                     item.ServerOptions.ProtocolVersions.First();

        return item;
    }
}