using System;

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
            // Add some demo entries.
            target.Items.Add(new ConnectionItemViewModel(target)
            {
                Name = "localhost",
                ServerOptions =
                {
                    Host = "localhost"
                }
            });

            target.Items.Add(new ConnectionItemViewModel(target)
            {
                Name = "Hive MQ",
                ServerOptions =
                {
                    Host = "broker.hivemq.com"
                }
            });

            target.Items.Add(new ConnectionItemViewModel(target)
            {
                Name = "Mosquitto Test",
                ServerOptions =
                {
                    Host = "test.mosquitto.org"
                }
            });
            
            return;
        }

        foreach (var connectionState in state.Connections)
        {
            target.Items.Add(new ConnectionItemViewModel(target)
            {
                Name = connectionState.Name ?? String.Empty,
                ServerOptions =
                {
                    Host = connectionState.Host ?? string.Empty,
                    Port = connectionState.Port
                }
            });
        }
    }
}