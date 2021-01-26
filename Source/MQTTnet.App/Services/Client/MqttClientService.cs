using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MQTTnet.App.Pages.Connection;
using MQTTnet.App.Pages.Subscriptions;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using MQTTnet.Diagnostics.PacketInspection;

namespace MQTTnet.App.Services.Client
{
    public sealed class MqttClientService : IMqttApplicationMessageReceivedHandler, IMqttPacketInspector
    {
        readonly List<IMqttApplicationMessageReceivedHandler> _applicationMessageReceivedHandlers = new List<IMqttApplicationMessageReceivedHandler>();
        readonly List<Action<ProcessMqttPacketContext>> _messageInspectors = new List<Action<ProcessMqttPacketContext>>();

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

            if (options.MqttNetOptions.EnablePacketInspection)
            {
                clientOptionsBuilder.WithPacketInspector(this);
            }

            await _mqttClient.ConnectAsync(clientOptionsBuilder.Build());

            IsConnected = true;
        }

        public async Task Subscribe(SubscriptionEditorViewModel options)
        {
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(options.Topic)
                .WithQualityOfServiceLevel(options.QualityOfServiceLevels.SelectedItem.Value)
                .WithNoLocal(options.NoLocal)
                .WithRetainAsPublished(options.RetainAsPublished)
                .Build();

            var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(topicFilter)
                .Build();

            var subscribeResult = await _mqttClient.SubscribeAsync(subscribeOptions).ConfigureAwait(false);


        }

        public void RegisterApplicationMessageReceivedHandler(IMqttApplicationMessageReceivedHandler handler)
        {
            _applicationMessageReceivedHandlers.Add(handler);
        }

        public void RegisterMessageInspectorHandler([NotNull] Action<ProcessMqttPacketContext> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _messageInspectors.Add(handler);
        }

        public async Task HandleApplicationMessageReceivedAsync([NotNull] MqttApplicationMessageReceivedEventArgs eventArgs)
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
    }
}
