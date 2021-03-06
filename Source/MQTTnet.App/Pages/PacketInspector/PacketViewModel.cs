using System;
using MQTTnet.App.Common;
using MQTTnet.App.Common.BufferInspector;
using MQTTnet.Diagnostics.PacketInspection;

namespace MQTTnet.App.Pages.PacketInspector
{
    public sealed class PacketViewModel : BaseViewModel
    {
        public PacketViewModel(int number, ProcessMqttPacketContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            Number = number;
            Name += GetControlPacketType(context.Buffer[0]);
            Length = context.Buffer.Length;

            Direction = context.Direction;
            IsInbound = context.Direction == MqttPacketFlowDirection.Inbound;

            ContentInspector = new BufferInspectorViewModel();
            ContentInspector.Dump(context.Buffer);
        }

        public int Number { get; }

        public int Length { get; }

        public string Name { get; }

        public bool IsInbound { get; }

        public MqttPacketFlowDirection Direction { get; set; }

        public BufferInspectorViewModel ContentInspector { get; }

        static string GetControlPacketType(byte data)
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