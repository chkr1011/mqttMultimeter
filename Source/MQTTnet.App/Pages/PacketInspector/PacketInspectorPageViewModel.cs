using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Client;
using MQTTnet.Diagnostics.PacketInspection;

namespace MQTTnet.App.Pages.PacketInspector
{
    public sealed class PacketInspectorPageViewModel : BasePageViewModel
    {
        int _number;

        public PacketInspectorPageViewModel(MqttClientService mqttClientService)
        {
            Header = "Packet Inspector";

            mqttClientService.RegisterMessageInspectorHandler(ProcessPacket);
        }

        void ProcessPacket(ProcessMqttPacketContext context)
        {
            var number = _number++;
            var viewModel = new PacketViewModel(number, context);

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Packets.Add(viewModel);
            });
        }

        public ViewModelCollection<PacketViewModel> Packets { get; } = new ViewModelCollection<PacketViewModel>();

        public void Clear()
        {
            _number = 0;
            Packets.Clear();
        }
    }
}
