using System.Security.Authentication;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class TlsVersionViewModel
    {
        public TlsVersionViewModel(string displayName, SslProtocols value)
        {
            DisplayName = displayName;
            Value = value;
        }

        public string DisplayName { get; }

        public SslProtocols Value { get; }
    }
}