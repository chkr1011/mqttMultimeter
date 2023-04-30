using System;
using System.Linq;

namespace mqttMultimeter.Pages.Connection.State;

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

    static ConnectionItemViewModel CreateItem(ConnectionPageViewModel ownerPage, ConnectionState state)
    {
        var item = new ConnectionItemViewModel(ownerPage)
        {
            Name = state.Name ?? string.Empty,
            ServerOptions =
            {
                Host = state.Host ?? string.Empty,
                Port = state.Port,
                IgnoreCertificateErrors = state.IgnoreCertificateErrors
            },
            SessionOptions =
            {
                UserName = state.UserName ?? string.Empty,
                KeepAliveInterval = state.KeepAliveInterval,
                AuthenticationMethod = state.AuthenticationMethod ?? string.Empty,
                CertificatePath = state.CertificatePath ?? string.Empty,
                CertificatePassword = state.CertificatePassword ?? string.Empty,

                // If there was a password saved in the state we assume that it should be saved (also in the future).
                SaveCertificatePassword = !string.IsNullOrEmpty(state.CertificatePassword)
            }
        };

        item.ServerOptions.SelectedTransport = item.ServerOptions.Transports.FirstOrDefault(t => t.Value.Equals(state.Transport)) ?? item.ServerOptions.Transports.First();

        item.ServerOptions.SelectedTlsVersion = item.ServerOptions.TlsVersions.FirstOrDefault(t => t.Value.Equals(state.TlsVersion)) ?? item.ServerOptions.TlsVersions.First();

        item.ServerOptions.SelectedProtocolVersion = item.ServerOptions.ProtocolVersions.FirstOrDefault(p => p.Value.Equals(state.ProtocolVersion)) ??
                                                     item.ServerOptions.ProtocolVersions.First();

        return item;
    }
}