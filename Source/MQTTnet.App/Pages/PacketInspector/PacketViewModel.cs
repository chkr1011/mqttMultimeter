using MQTTnet.App.Common;
using MQTTnet.App.Common.BufferInspector;
using MQTTnet.Diagnostics.PacketInspection;

namespace MQTTnet.App.Pages.PacketInspector
{
    public sealed class PacketViewModel : BaseViewModel
    {
        public PacketViewModel(ProcessMqttPacketContext context)
        {
            Direction = context.Direction;

            if (context.Direction == MqttPacketFlowDirection.Inbound)
            {
                Name = ">>> ";
            }
            else
            {
                Name = "<<< ";
            }

            Name += GetControlPacketType(context.Buffer[0]);

            ContentInspector = new BufferInspectorViewModel(context.Buffer);
        }

        public string Name { get; set; }

        public MqttPacketFlowDirection Direction { get; set; }

        public BufferInspectorViewModel ContentInspector { get; }

        string GetControlPacketType(byte data)
        {
            var controlType = data >> 4;

            switch (controlType)
            {
                case 1: return "CONNECT";
                case 2: return "CONNACK";
                case 3: return "PUBLISH";
                case 4: return "PUBACK";
                case 5: return "PUBREC";
                case 6: return "PUBREL";
                case 7: return "PUBCOMP";
                case 8: return "SUBSCRIBE";
                case 9: return "SUBACK";
                case 10: return "UNSUBSCRIBE";
                case 11: return "UNSUBACK";
                case 12: return "PINGREQ";
                case 13: return "PINGRESP";
                case 14: return "DISCONNECT";
                case 15: return "AUTH";
                default: return "UNKNOWN";
            }
        }
    }
}