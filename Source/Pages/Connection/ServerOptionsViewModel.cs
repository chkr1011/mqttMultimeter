using System.Collections.ObjectModel;
using System.Security.Authentication;
using mqttMultimeter.Common;
using MQTTnet.Formatter;
using ReactiveUI;

namespace mqttMultimeter.Pages.Connection;

public sealed class ServerOptionsViewModel : BaseViewModel
{
    EnumViewModel<MqttProtocolVersion> _selectedProtocolVersion;
    EnumViewModel<SslProtocols> _selectedTlsVersion;
    TransportViewModel _selectedTransport;

    public ServerOptionsViewModel()
    {
        Host = "localhost";
        Port = 1883;
        CommunicationTimeout = 10;

        Transports.Add(new TransportViewModel("TCP", Transport.Tcp)
        {
            IsPortAvailable = true,
            HostDisplayValue = "Host"
        });

        Transports.Add(new TransportViewModel("WebSocket", Transport.WebSocket)
        {
            HostDisplayValue = "URI"
        });

        _selectedTransport = Transports[0];

        TlsVersions.Add(new EnumViewModel<SslProtocols>("no TLS", SslProtocols.None));
#pragma warning disable SYSLIB0039
        TlsVersions.Add(new EnumViewModel<SslProtocols>("TLS 1.0", SslProtocols.Tls));
        TlsVersions.Add(new EnumViewModel<SslProtocols>("TLS 1.1", SslProtocols.Tls11));
#pragma warning restore SYSLIB0039
        TlsVersions.Add(new EnumViewModel<SslProtocols>("TLS 1.2", SslProtocols.Tls12));
        TlsVersions.Add(new EnumViewModel<SslProtocols>("TLS 1.3", SslProtocols.Tls13));
        _selectedTlsVersion = TlsVersions[0];

        ProtocolVersions.Add(new EnumViewModel<MqttProtocolVersion>("3.1.0", MqttProtocolVersion.V310));
        ProtocolVersions.Add(new EnumViewModel<MqttProtocolVersion>("3.1.1", MqttProtocolVersion.V311));
        ProtocolVersions.Add(new EnumViewModel<MqttProtocolVersion>("5.0.0", MqttProtocolVersion.V500));

        // 3.1.1 is the mostly used version so we preselect it.
        _selectedProtocolVersion = ProtocolVersions[1];
    }

    public int CommunicationTimeout
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string Host
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public bool IgnoreCertificateErrors
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public int Port
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObservableCollection<EnumViewModel<MqttProtocolVersion>> ProtocolVersions { get; } = [];

    public ushort ReceiveMaximum
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public EnumViewModel<MqttProtocolVersion> SelectedProtocolVersion
    {
        get => _selectedProtocolVersion;
        set => this.RaiseAndSetIfChanged(ref _selectedProtocolVersion, value);
    }

    public EnumViewModel<SslProtocols> SelectedTlsVersion
    {
        get => _selectedTlsVersion;
        set => this.RaiseAndSetIfChanged(ref _selectedTlsVersion, value);
    }

    public TransportViewModel SelectedTransport
    {
        get => _selectedTransport;
        set => this.RaiseAndSetIfChanged(ref _selectedTransport, value);
    }

    public ObservableCollection<EnumViewModel<SslProtocols>> TlsVersions { get; } = [];

    public ObservableCollection<TransportViewModel> Transports { get; } = [];
}