using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using mqttMultimeter.Common;
using mqttMultimeter.Services.Mqtt;
using MQTTnet.Diagnostics.PacketInspection;
using ReactiveUI;

namespace mqttMultimeter.Pages.PacketInspector;

public sealed class PacketInspectorPageViewModel : BasePageViewModel
{
    bool _isRecordingEnabled;
    int _number;
    PacketViewModel? _selectedPacket;

    public PacketInspectorPageViewModel(MqttClientService mqttClientService)
    {
        if (mqttClientService == null)
        {
            throw new ArgumentNullException(nameof(mqttClientService));
        }

        mqttClientService.RegisterMessageInspectorHandler(ProcessPacket);
    }

    public bool IsRecordingEnabled
    {
        get => _isRecordingEnabled;
        set => this.RaiseAndSetIfChanged(ref _isRecordingEnabled, value);
    }

    public ObservableCollection<PacketViewModel> Packets { get; } = new();

    public PacketViewModel? SelectedPacket
    {
        get => _selectedPacket;
        set => this.RaiseAndSetIfChanged(ref _selectedPacket, value);
    }

    public void ClearItems()
    {
        _number = 0;
        Packets.Clear();
    }

    static string GetControlPacketType(byte data)
    {
        var controlType = data >> 4;

        switch (controlType)
        {
            case 1:
                return "CONNECT (01)";
            case 2:
                return "CONNACK (02)";
            case 3:
                return "PUBLISH (03)";
            case 4:
                return "PUBACK (04)";
            case 5:
                return "PUBREC (05)";
            case 6:
                return "PUBREL (06)";
            case 7:
                return "PUBCOMP (07)";
            case 8:
                return "SUBSCRIBE (08)";
            case 9:
                return "SUBACK (09)";
            case 10:
                return "UNSUBSCRIBE (10)";
            case 11:
                return "UNSUBACK (11)";
            case 12:
                return "PINGREQ (12)";
            case 13:
                return "PINGRESP (13)";
            case 14:
                return "DISCONNECT (14)";
            case 15:
                return "AUTH (15)";
            default:
                return "UNKNOWN";
        }
    }

    Task ProcessPacket(InspectMqttPacketEventArgs eventArgs)
    {
        if (!_isRecordingEnabled)
        {
            return Task.CompletedTask;
        }

        Dispatcher.UIThread.Invoke(() =>
        {
            var number = _number++;
            var viewModel = new PacketViewModel
            {
                Number = number,
                Type = GetControlPacketType(eventArgs.Buffer[0]),
                Data = eventArgs.Buffer,
                Length = eventArgs.Buffer.Length,
                IsInbound = eventArgs.Direction == MqttPacketFlowDirection.Inbound
            };

            Packets.Add(viewModel);

            // TODO: Move to configuration.
            if (Packets.Count > 1000)
            {
                Packets.RemoveAt(0);
            }
        });

        return Task.CompletedTask;
    }
}