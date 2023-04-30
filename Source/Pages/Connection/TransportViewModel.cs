using mqttMultimeter.Common;

namespace mqttMultimeter.Pages.Connection;

public sealed class TransportViewModel : EnumViewModel<Transport>
{
    public TransportViewModel(object? displayValue, Transport value) : base(displayValue, value)
    {
    }

    public object? HostDisplayValue { get; set; }

    public bool IsPortAvailable { get; set; }
}