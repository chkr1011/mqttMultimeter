using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using MQTTnet.Adapter;
using MQTTnet.App.Common;
using MQTTnet.App.Common.ObjectDump;
using MQTTnet.App.Services.Client;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class ConnectionPageViewModel : BasePageViewModel
    {
        readonly ConnectionPageHeaderViewModel _header = new ConnectionPageHeaderViewModel();

        readonly MqttClientService _mqttClientService;

        public ConnectionPageViewModel(MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService;

            Header = _header;

            var timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, TimerCallback);
            timer.Start();
        }

        public ProtocolOptionsViewModel ProtocolOptions { get; } = new ProtocolOptionsViewModel();

        public ServerOptionsViewModel ServerOptions { get; } = new ServerOptionsViewModel();

        public SessionOptionsViewModel SessionOptions { get; } = new SessionOptionsViewModel();

        public MqttNetOptionsViewModel MqttNetOptions { get; } = new MqttNetOptionsViewModel();

        public ObjectDumpViewModel Result { get; } = new ObjectDumpViewModel();

        public bool IsConnecting
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public async Task Connect()
        {
            try
            {
                IsConnecting = true;

                var result = await _mqttClientService.Connect(this);

                Result.Dump(result);
            }
            catch (MqttConnectingFailedException exception)
            {
                Result.Dump(exception.Result);
            }
            catch (Exception exception)
            {
                App.ShowException(exception);
            }
            finally
            {
                IsConnecting = false;
            }
        }

        public async Task Disconnect()
        {
            try
            {
                await _mqttClientService.Disconnect();
            }
            catch (Exception exception)
            {
                App.ShowException(exception);
            }
        }

        void TimerCallback(object? sender, EventArgs e)
        {
            _header.IsConnected = _mqttClientService.IsConnected;
        }
    }
}
