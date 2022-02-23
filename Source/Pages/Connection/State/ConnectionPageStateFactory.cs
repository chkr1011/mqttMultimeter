using System;

namespace MQTTnetApp.Pages.Connection.State;

public static class ConnectionPageStateFactory
{
    public static ConnectionPageState Create(ConnectionPageViewModel viewModel)
    {
        if (viewModel == null)
        {
            throw new ArgumentNullException(nameof(viewModel));
        }

        var state = new ConnectionPageState();

        foreach (var item in viewModel.Items.Collection)
        {
            state.Connections.Add(new ConnectionState
            {
                Name = item.Name,
                Host = item.ServerOptions.Host,
                Port = item.ServerOptions.Port,
                ClientId = item.SessionOptions.ClientId,
                UserName = item.SessionOptions.UserName,
                AuthenticationMethod = item.SessionOptions.AuthenticationMethod,
                ProtocolVersion = item.ServerOptions.SelectedProtocolVersion.Value,
                KeepAliveInterval = item.SessionOptions.KeepAliveInterval,
                Transport = item.ServerOptions.SelectedTransport.Value,
                TlsVersion = item.ServerOptions.SelectedTlsVersion.Value
            });
        }

        return state;
    }
}