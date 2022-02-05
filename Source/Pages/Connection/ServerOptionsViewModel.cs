using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Authentication;
using MQTTnet.Formatter;
using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.Connection;

public sealed class ServerOptionsViewModel : BaseViewModel
{
    int _communicationTimeout;
    string _host = string.Empty;
    int _port;
    int _receiveMaximum;
    EnumViewModel<MqttProtocolVersion>? _selectedProtocolVersion;

    EnumViewModel<SslProtocols>? _selectedTlsVersion;
    EnumViewModel<Transport>? _selectedTransport;

    public ServerOptionsViewModel()
    {
        Host = "localhost";
        Port = 1883;
        CommunicationTimeout = 10;

        Transports.Add(new EnumViewModel<Transport>("TCP", Transport.TCP));
        Transports.Add(new EnumViewModel<Transport>("WebSocket", Transport.WebSocket));
        SelectedTransport = Transports.FirstOrDefault()!;

        TlsVersions.Add(new EnumViewModel<SslProtocols>("no TLS", SslProtocols.None));
        TlsVersions.Add(new EnumViewModel<SslProtocols>("TLS 1.0", SslProtocols.Tls));
        TlsVersions.Add(new EnumViewModel<SslProtocols>("TLS 1.1", SslProtocols.Tls11));
        TlsVersions.Add(new EnumViewModel<SslProtocols>("TLS 1.2", SslProtocols.Tls12));
        TlsVersions.Add(new EnumViewModel<SslProtocols>("TLS 1.3", SslProtocols.Tls13));
        SelectedTlsVersion = TlsVersions.FirstOrDefault()!;

        ProtocolVersions.Add(new EnumViewModel<MqttProtocolVersion>("3.1.0", MqttProtocolVersion.V310));
        ProtocolVersions.Add(new EnumViewModel<MqttProtocolVersion>("3.1.1", MqttProtocolVersion.V311));
        ProtocolVersions.Add(new EnumViewModel<MqttProtocolVersion>("5.0.0", MqttProtocolVersion.V500));

        // 3.1.1 is the mostly used version so we preselect it.
        SelectedProtocolVersion = ProtocolVersions[1];
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

    public ObservableCollection<EnumViewModel<MqttProtocolVersion>> ProtocolVersions { get; } = new();

    public int ReceiveMaximum
    {
        get => _receiveMaximum;
        set => this.RaiseAndSetIfChanged(ref _receiveMaximum, value);
    }

    public EnumViewModel<MqttProtocolVersion>? SelectedProtocolVersion
    {
        get => _selectedProtocolVersion;
        set => this.RaiseAndSetIfChanged(ref _selectedProtocolVersion, value);
    }

    public EnumViewModel<SslProtocols>? SelectedTlsVersion
    {
        get => _selectedTlsVersion;
        set => this.RaiseAndSetIfChanged(ref _selectedTlsVersion, value);
    }

    public EnumViewModel<Transport>? SelectedTransport
    {
        get => _selectedTransport;
        set => this.RaiseAndSetIfChanged(ref _selectedTransport, value);
    }

    public ObservableCollection<EnumViewModel<SslProtocols>> TlsVersions { get; } = new();

    public ObservableCollection<EnumViewModel<Transport>> Transports { get; } = new();
}