using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using MQTTnet.App.Pages.Connection;
using MQTTnet.App.Pages.Publish;
using MQTTnet.App.Pages.Subscriptions;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Internal;

namespace MQTTnet.App.Services.Mqtt;

public sealed class MqttClientService : IMqttPacketInspector
{
    readonly AsyncEvent<MqttApplicationMessageReceivedEventArgs> _applicationMessageReceivedEvent = new();
    readonly List<Action<ProcessMqttPacketContext>> _messageInspectors = new();

    IMqttClient? _mqttClient;

    public event Func<MqttApplicationMessageReceivedEventArgs, Task> ApplicationMessageReceived
    {
        add => _applicationMessageReceivedEvent.AddHandler(value);
        remove => _applicationMessageReceivedEvent.RemoveHandler(value);
    }

    public bool IsConnected => _mqttClient?.IsConnected == true;

    public async Task<MqttClientConnectResult> Connect(ConnectionPageViewModel options)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (_mqttClient != null)
        {
            await _mqttClient.DisconnectAsync();
            _mqttClient.Dispose();
        }

        _mqttClient = new MqttFactory().CreateMqttClient();

        var clientOptionsBuilder = new MqttClientOptionsBuilder().WithCommunicationTimeout(TimeSpan.FromSeconds(options.ServerOptions.CommunicationTimeout))
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

        // TODO: To event.
        if (options.MqttNetOptions.EnablePacketInspection)
        {
            clientOptionsBuilder.WithPacketInspector(this);
        }

        _mqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceived;

        var result = await _mqttClient.ConnectAsync(clientOptionsBuilder.Build());

        return result;
    }

    public Task Disconnect()
    {
        ThrowIfNotConnected();

        return _mqttClient.DisconnectAsync();
    }

    public void ProcessMqttPacket(ProcessMqttPacketContext context)
    {
        foreach (var messageInspector in _messageInspectors)
        {
            messageInspector.Invoke(context);
        }
    }

    public Task<MqttClientPublishResult> Publish(PublishItemViewModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        ThrowIfNotConnected();

        var applicationMessageBuilder = new MqttApplicationMessageBuilder().WithTopic(item.Topic)
            .WithQualityOfServiceLevel(item.QualityOfServiceLevel.Value)
            .WithRetainFlag(item.Retain)
            .WithMessageExpiryInterval(item.MessageExpiryInterval)
            .WithContentType(item.ContentType)
            .WithPayloadFormatIndicator(item.PayloadFormatIndicator.ToPayloadFormatIndicator())
            .WithPayload(item.PayloadFormatIndicator.ToPayload(item.Payload))
            .WithSubscriptionIdentifier(item.SubscriptionIdentifier)
            .WithResponseTopic(item.ResponseTopic);

        if (item.TopicAlias > 0)
        {
            applicationMessageBuilder.WithTopicAlias(item.TopicAlias);
        }

        foreach (var userProperty in item.UserProperties.Items)
        {
            if (!string.IsNullOrEmpty(userProperty.Name))
            {
                applicationMessageBuilder.WithUserProperty(userProperty.Name, userProperty.Value);
            }
        }

        return _mqttClient.PublishAsync(applicationMessageBuilder.Build());
    }

    public void RegisterMessageInspectorHandler(Action<ProcessMqttPacketContext> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        _messageInspectors.Add(handler);
    }

    public async Task<MqttClientSubscribeResult> Subscribe(SubscriptionOptionsPageViewModel configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        ThrowIfNotConnected();

        var topicFilter = new MqttTopicFilterBuilder().WithTopic(configuration.Topic)
            .WithQualityOfServiceLevel(configuration.QualityOfServiceLevel.Value)
            .WithNoLocal(configuration.NoLocal)
            .WithRetainHandling(configuration.RetainHandling)
            .WithRetainAsPublished(configuration.RetainAsPublished)
            .Build();

        var subscribeOptions = new MqttClientSubscribeOptionsBuilder().WithTopicFilter(topicFilter).Build();

        var subscribeResult = await _mqttClient.SubscribeAsync(subscribeOptions).ConfigureAwait(false);

        return subscribeResult;
    }

    public async Task<MqttClientUnsubscribeResult> Unsubscribe(string topic)
    {
        if (topic == null)
        {
            throw new ArgumentNullException(nameof(topic));
        }

        ThrowIfNotConnected();

        var unsubscribeResult = await _mqttClient.UnsubscribeAsync(topic);

        return unsubscribeResult;
    }

    Task OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        return _applicationMessageReceivedEvent.InvokeAsync(eventArgs);
    }

    void ThrowIfNotConnected()
    {
        if (_mqttClient == null || !_mqttClient.IsConnected)
        {
            throw new InvalidOperationException("The MQTT client is not connected.");
        }
    }
}