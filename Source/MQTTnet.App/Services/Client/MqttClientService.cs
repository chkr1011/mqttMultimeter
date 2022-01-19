using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using MQTTnet.App.Pages.Connection;
using MQTTnet.App.Pages.Publish;
using MQTTnet.App.Pages.Subscriptions;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;
using MQTTnet.Diagnostics.PacketInspection;

namespace MQTTnet.App.Services.Client
{
    public sealed class MqttClientService : IMqttApplicationMessageReceivedHandler, IMqttPacketInspector
    {
        readonly List<IMqttApplicationMessageReceivedHandler> _applicationMessageReceivedHandlers = new();
        readonly List<Action<ProcessMqttPacketContext>> _messageInspectors = new();

        IMqttClient? _mqttClient;

        public bool IsConnected => _mqttClient?.IsConnected == true;

        public async Task<MqttClientConnectResult> Connect(ConnectionPageViewModel options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (_mqttClient != null)
            {
                await _mqttClient.DisconnectAsync();
                _mqttClient.Dispose();
            }

            _mqttClient = new MqttFactory().CreateMqttClient();

            _mqttClient.UseApplicationMessageReceivedHandler(this);

            var clientOptionsBuilder = new MqttClientOptionsBuilder()
                .WithCommunicationTimeout(TimeSpan.FromSeconds(options.ServerOptions.CommunicationTimeout))
                .WithProtocolVersion(options.ProtocolOptions.ProtocolVersions.SelectedItem.Value)
                .WithClientId(options.SessionOptions.ClientId)
                .WithCleanSession(options.SessionOptions.CleanSession)
                .WithCredentials(options.SessionOptions.User, options.SessionOptions.Password)
                .WithRequestProblemInformation(options.SessionOptions.RequestProblemInformation)
                .WithRequestResponseInformation(options.SessionOptions.RequestResponseInformation)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(options.SessionOptions.KeepAliveInterval));

            if (options.ServerOptions.Transports.SelectedItem.Transport == Transport.TCP)
            {
                clientOptionsBuilder.WithTcpServer(options.ServerOptions.Host, options.ServerOptions.Port);
            }
            else
            {
                clientOptionsBuilder.WithWebSocketServer(options.ServerOptions.Host);
            }

            if (options.ServerOptions.TlsVersions.SelectedItem.Value != SslProtocols.None)
            {
                clientOptionsBuilder.WithTls(o =>
                {
                    o.SslProtocol = options.ServerOptions.TlsVersions.SelectedItem.Value;
                });
            }

            if (options.MqttNetOptions.EnablePacketInspection)
            {
                clientOptionsBuilder.WithPacketInspector(this);
            }

            var result = await _mqttClient.ConnectAsync(clientOptionsBuilder.Build());

            return result;
        }

        public Task Disconnect()
        {
            ThrowIfNotConnected();

            return _mqttClient.DisconnectAsync();
        }

        public async Task<MqttClientSubscribeResult> Subscribe(SubscriptionOptionsPageViewModel configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            ThrowIfNotConnected();

            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(configuration.Topic)
                .WithQualityOfServiceLevel(configuration.QualityOfServiceLevel.Value)
                .WithNoLocal(configuration.NoLocal)
                .WithRetainHandling(configuration.RetainHandling)
                .WithRetainAsPublished(configuration.RetainAsPublished)
                .Build();

            var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(topicFilter)
                .Build();

            var subscribeResult = await _mqttClient.SubscribeAsync(subscribeOptions).ConfigureAwait(false);

            return subscribeResult;
        }

        public Task<MqttClientPublishResult> Publish(PublishOptionsViewModel options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            ThrowIfNotConnected();

            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(options.Topic)
                .WithQualityOfServiceLevel(options.QualityOfServiceLevel.Value)
                .WithRetainFlag(options.Retain)
                .WithPayload(options.GeneratePayload())
                .Build();

            return _mqttClient.PublishAsync(applicationMessage);
        }

        public async Task<MqttClientUnsubscribeResult> Unsubscribe(string topic)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));

            ThrowIfNotConnected();

            var unsubscribeResult = await _mqttClient.UnsubscribeAsync(topic);

            return unsubscribeResult;
        }

        public void RegisterApplicationMessageReceivedHandler(IMqttApplicationMessageReceivedHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _applicationMessageReceivedHandlers.Add(handler);
        }

        public void RegisterMessageInspectorHandler(Action<ProcessMqttPacketContext> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _messageInspectors.Add(handler);
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            foreach (var handler in _applicationMessageReceivedHandlers)
            {
                await handler.HandleApplicationMessageReceivedAsync(eventArgs);
            }
        }

        public void ProcessMqttPacket(ProcessMqttPacketContext context)
        {
            foreach (var messageInspector in _messageInspectors)
            {
                messageInspector.Invoke(context);
            }
        }

        void ThrowIfNotConnected()
        {
            if (_mqttClient == null || !_mqttClient.IsConnected)
            {
                throw new InvalidOperationException("The MQTT client is not connected.");
            }
        }
    }
}
