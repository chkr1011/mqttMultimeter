namespace MQTTnet.App.Pages.Connection;

public sealed class TransportViewModel
{
    public TransportViewModel(string displayName, Transport value)
    {
        DisplayName = displayName;
        Value = value;
    }

    public string DisplayName { get; }

    public Transport Value { get; }
}