using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using MQTTnet.App.Pages.Connection;
using MQTTnet.App.Pages.Subscriptions;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;

namespace MQTTnet.App.Services.Client
{
    public sealed class MqttClientService : IMqttApplicationMessageReceivedHandler
    {
        readonly List<IMqttApplicationMessageReceivedHandler> _applicationMessageReceivedHandlers = new List<IMqttApplicationMessageReceivedHandler>();

        IMqttClient? _mqttClient;

        public bool IsConnected { get; private set; }

        public async Task Connect(ConnectionPageViewModel options)
        {
            IsConnected = false;

            if (_mqttClient != null)
            {
                await _mqttClient.DisconnectAsync();
                _mqttClient.Dispose();
            }

            _mqttClient = new MqttFactory().CreateMqttClient();

            _mqttClient.UseApplicationMessageReceivedHandler(this);

            var clientOptionsBuilder = new MqttClientOptionsBuilder()
                .WithProtocolVersion(options.ProtocolOptions.ProtocolVersions.SelectedItem.Value)
                .WithClientId(options.SessionOptions.ClientId)
                .WithCredentials(options.SessionOptions.User, options.SessionOptions.Password)
                .WithTcpServer(options.ServerOptions.Host, options.ServerOptions.Port)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(options.ServerOptions.KeepAliveInterval));

            if (options.ServerOptions.TlsVersions.SelectedItem.Value != SslProtocols.None)
            {
                clientOptionsBuilder.WithTls(o =>
                {
                    o.SslProtocol = options.ServerOptions.TlsVersions.SelectedItem.Value;
                });
            }
            
            await _mqttClient.ConnectAsync(clientOptionsBuilder.Build());

            await _mqttClient.SubscribeAsync("#");

            IsConnected = true;
        }

        public async Task Subscribe()
        {
            var subscribeOptionsBuilder = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(new MqttTopicFilterBuilder().WithTopic(""));

            //_mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder())
        }

        public void RegisterApplicationMessageReceivedHandler(IMqttApplicationMessageReceivedHandler handler)
        {
            _applicationMessageReceivedHandlers.Add(handler);
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            foreach (var handler in _applicationMessageReceivedHandlers)
            {
                await handler.HandleApplicationMessageReceivedAsync(eventArgs);
            }
        }
    }
}
