using System;
using System.Threading.Tasks;
using mqttMultimeter.Common;
using ReactiveUI;

namespace mqttMultimeter.Pages.Connection;

public sealed class ConnectionItemViewModel : BaseViewModel
{
    string _name = string.Empty;

    public ConnectionItemViewModel(ConnectionPageViewModel ownerPage)
    {
        OwnerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));

        SessionOptions.UserProperties.AddEmptyItem();
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ConnectionPageViewModel OwnerPage { get; }

    public ConnectResponseViewModel Response { get; } = new();

    public ServerOptionsViewModel ServerOptions { get; } = new();

    public SessionOptionsViewModel SessionOptions { get; } = new();

    public Task Connect()
    {
        return OwnerPage.Connect(this);
    }

    public Task Disconnect()
    {
        return OwnerPage.Disconnect(this);
    }
}