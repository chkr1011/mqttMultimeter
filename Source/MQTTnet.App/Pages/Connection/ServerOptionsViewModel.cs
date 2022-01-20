using System.Linq;
using System.Security.Authentication;
using MQTTnet.App.Common;

namespace MQTTnet.App.Pages.Connection;

public sealed class ServerOptionsViewModel : BaseViewModel
{
    public ServerOptionsViewModel()
    {
        Host = "localhost";
        Port = 1883;
        CommunicationTimeout = 10;

        Transports.Add(new TransportViewModel("TCP", Transport.TCP));
        Transports.Add(new TransportViewModel("WebSocket", Transport.WebSocket));
        Transports.SelectedItem = Transports.FirstOrDefault()!;

        TlsVersions.Add(new TlsVersionViewModel("no TLS", SslProtocols.None));
        TlsVersions.Add(new TlsVersionViewModel("TLS 1.0", SslProtocols.Tls));
        TlsVersions.Add(new TlsVersionViewModel("TLS 1.1", SslProtocols.Tls11));
        TlsVersions.Add(new TlsVersionViewModel("TLS 1.2", SslProtocols.Tls12));
        TlsVersions.Add(new TlsVersionViewModel("TLS 1.3", SslProtocols.Tls13));

        TlsVersions.SelectedItem = TlsVersions.FirstOrDefault()!;
    }

    public int CommunicationTimeout { get; set; }

    public string Host
    {
        get => GetValue<string>();
        set => SetValue(value);
    }

    public int Port
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public ViewModelCollection<TlsVersionViewModel> TlsVersions { get; } = new();

    public ViewModelCollection<TransportViewModel> Transports { get; } = new();
}