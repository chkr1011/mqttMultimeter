using MQTTnet.App.Common;
using MQTTnet.Diagnostics.PacketInspection;

namespace MQTTnet.App.Pages.PacketInspector
{
    public sealed class PacketViewModel : BaseViewModel
    {
        public PacketViewModel(ProcessMqttPacketContext context)
        {
            Direction = context.Direction;

            ContentInspector = new BufferInspectorViewModel(context.Buffer);
        }

        public MqttPacketFlowDirection Direction { get; set; }

        public BufferInspectorViewModel ContentInspector { get; }
    }
}