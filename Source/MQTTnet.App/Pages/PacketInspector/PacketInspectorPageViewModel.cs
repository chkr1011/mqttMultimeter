using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Client;
using MQTTnet.Diagnostics.PacketInspection;

namespace MQTTnet.App.Pages.PacketInspector
{
    public sealed class PacketInspectorPageViewModel : BasePageViewModel
    {
        public PacketInspectorPageViewModel(MqttClientService mqttClientService)
        {
            Header = "Packet Inspector";

            mqttClientService.RegisterMessageInspectorHandler(ProcessPacket);
        }

        void ProcessPacket(ProcessMqttPacketContext context)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Packets.Add(new PacketViewModel(context));
            });
        }

        public ViewModelCollection<PacketViewModel> Packets { get; } = new();
    }
}
