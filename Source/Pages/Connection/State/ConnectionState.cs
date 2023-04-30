using System.Security.Authentication;
using MQTTnet.Formatter;

namespace mqttMultimeter.Pages.Connection.State;

public sealed class ConnectionState
{
    public string? AuthenticationMethod { get; set; }

    public string? CertificatePassword { get; set; }

    public string? CertificatePath { get; set; }

    public string? ClientId { get; set; }

    public string? Host { get; set; }

    public bool IgnoreCertificateErrors { get; set; }

    public int KeepAliveInterval { get; set; }

    public string? Name { get; set; }

    public int Port { get; set; }

    public MqttProtocolVersion ProtocolVersion { get; set; }

    public SslProtocols TlsVersion { get; set; }

    public Transport Transport { get; set; }

    public string? UserName { get; set; }
}