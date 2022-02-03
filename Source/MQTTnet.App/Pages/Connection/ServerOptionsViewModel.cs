using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Authentication;
using MQTTnet.App.Common;
using ReactiveUI;

namespace MQTTnet.App.Pages.Connection;

public sealed class ServerOptionsViewModel : BaseViewModel
{
    int _communicationTimeout;
    string _host = string.Empty;
    int _port;

    TlsVersionViewModel? _selectedTlsVersion;

    TransportViewModel? _selectedTransport;

    public ServerOptionsViewModel()
    {
        Host = "localhost";
        Port = 1883;
        CommunicationTimeout = 10;

        Transports.Add(new TransportViewModel("TCP", Transport.TCP));
        Transports.Add(new TransportViewModel("WebSocket", Transport.WebSocket));
        SelectedTransport = Transports.FirstOrDefault()!;

        TlsVersions.Add(new TlsVersionViewModel("no TLS", SslProtocols.None));
        TlsVersions.Add(new TlsVersionViewModel("TLS 1.0", SslProtocols.Tls));
        TlsVersions.Add(new TlsVersionViewModel("TLS 1.1", SslProtocols.Tls11));
        TlsVersions.Add(new TlsVersionViewModel("TLS 1.2", SslProtocols.Tls12));
        TlsVersions.Add(new TlsVersionViewModel("TLS 1.3", SslProtocols.Tls13));
        SelectedTlsVersion = TlsVersions.FirstOrDefault()!;
    }

    public int CommunicationTimeout
    {
        get => _communicationTimeout;
        set => this.RaiseAndSetIfChanged(ref _communicationTimeout, value);
    }

    public string Host
    {
        get => _host;
        set => this.RaiseAndSetIfChanged(ref _host, value);
    }

    public int Port
    {
        get => _port;
        set => this.RaiseAndSetIfChanged(ref _port, value);
    }

    public TlsVersionViewModel? SelectedTlsVersion
    {
        get => _selectedTlsVersion;
        set => this.RaiseAndSetIfChanged(ref _selectedTlsVersion, value);
    }

    public TransportViewModel? SelectedTransport
    {
        get => _selectedTransport;
        set => this.RaiseAndSetIfChanged(ref _selectedTransport, value);
    }

    public ObservableCollection<TlsVersionViewModel> TlsVersions { get; } = new();

    public ObservableCollection<TransportViewModel> Transports { get; } = new();
}