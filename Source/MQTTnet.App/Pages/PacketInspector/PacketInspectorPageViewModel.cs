using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Mqtt;
using MQTTnet.Diagnostics;

namespace MQTTnet.App.Pages.PacketInspector;

public sealed class PacketInspectorPageViewModel : BaseViewModel
{
    int _number;

    public PacketInspectorPageViewModel(MqttClientService mqttClientService)
    {
        mqttClientService.RegisterMessageInspectorHandler(ProcessPacket);
    }

    public ViewModelCollection<PacketViewModel> Packets { get; } = new();

    public void ClearItems()
    {
        _number = 0;
        Packets.Clear();
    }

    void ProcessPacket(ProcessMqttPacketContext context)
    {
        var number = _number++;
        var viewModel = new PacketViewModel
        {
            Number = number,
            Name = GetControlPacketType(context.Buffer[0]),
            Length = context.Buffer.Length,
            IsInbound = context.Direction == MqttPacketFlowDirection.Inbound
        };
        
        viewModel.ContentInspector.Dump(context.Buffer);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Packets.Add(viewModel);
        });
    }
    
    static string GetControlPacketType(byte data)
    {
        var controlType = data >> 4;

        switch (controlType)
        {
            case 1:
                return "CONNECT";
            case 2:
                return "CONNACK";
            case 3:
                return "PUBLISH";
            case 4:
                return "PUBACK";
            case 5:
                return "PUBREC";
            case 6:
                return "PUBREL";
            case 7:
                return "PUBCOMP";
            case 8:
                return "SUBSCRIBE";
            case 9:
                return "SUBACK";
            case 10:
                return "UNSUBSCRIBE";
            case 11:
                return "UNSUBACK";
            case 12:
                return "PINGREQ";
            case 13:
                return "PINGRESP";
            case 14:
                return "DISCONNECT";
            case 15:
                return "AUTH";
            default:
                return "UNKNOWN";
        }
    }
}