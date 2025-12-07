using System;
using System.Threading.Tasks;
using mqttMultimeter.Common;
using ReactiveUI;

namespace mqttMultimeter.Pages.Connection;

public sealed class ConnectionItemViewModel : BaseViewModel
{
    public ConnectionItemViewModel(ConnectionPageViewModel ownerPage)
    {
        OwnerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));

        SessionOptions.UserProperties.AddEmptyItem();

        Name = string.Empty;
    }

    public string Name
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
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