namespace MQTTnet.App.Pages.Connection
{
    public sealed class TransportViewModel
    {
        public TransportViewModel(string displayName, Transport transport)
        {
            DisplayName = displayName;
            Transport = transport;
        }

        public string DisplayName { get; }

        public Transport Transport { get; }
    }
}