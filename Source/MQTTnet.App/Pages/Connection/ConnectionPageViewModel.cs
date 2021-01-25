using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Client;

namespace MQTTnet.App.Pages.Connection
{
    public sealed class ConnectionPageViewModel : BasePageViewModel
    {
        readonly MqttClientService _mqttClientService;

        public ConnectionPageViewModel(MqttClientService mqttClientService)
        {
            _mqttClientService = mqttClientService;

            Header = "Connection";

            var timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, TimerCallback);
            timer.Start();
        }

        void TimerCallback(object? sender, EventArgs e)
        {

        }

        public ProtocolOptionsViewModel ProtocolOptions { get; } = new ProtocolOptionsViewModel();

        public ServerOptionsViewModel ServerOptions { get; } = new ServerOptionsViewModel();

        public SessionOptionsViewModel SessionOptions { get; } = new SessionOptionsViewModel();

        public MqttNetOptionsViewModel MqttNetOptions { get; } = new MqttNetOptionsViewModel();

        public string ErrorMessage
        {
            get => GetValue<string>();
            private set => SetValue(value);
        }

        public bool IsConnecting
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public async Task Connect()
        {
            try
            {
                ErrorMessage = string.Empty;
                IsConnecting = true;

                await _mqttClientService.Connect(this);
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
            }
            finally
            {
                IsConnecting = false;
            }
        }
    }
}
