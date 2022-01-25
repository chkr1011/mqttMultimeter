using MQTTnet.App.Common;
using MQTTnet.App.Controls;

namespace MQTTnet.App.Pages.PacketInspector;

public sealed class PacketViewModel : BaseViewModel
{
    public BufferInspectorViewModel ContentInspector { get; } = new();

    public bool IsInbound { get; set; }

    public int Length { get; set; }

    public string? Name { get; set; }

    public int Number { get; set; }
}