using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using mqttMultimeter.Controls;
using mqttMultimeter.Pages.Connection;
using mqttMultimeter.Pages.Publish;
using mqttMultimeter.Pages.Subscriptions;
using MQTTnet;
using MQTTnet.Diagnostics.Logger;
using MQTTnet.Diagnostics.PacketInspection;
using MQTTnet.Exceptions;
using MQTTnet.Internal;

namespace mqttMultimeter.Services.Mqtt;

public sealed class MqttClientService
{
    readonly AsyncEvent<MqttApplicationMessageReceivedEventArgs> _applicationMessageReceivedEvent = new();
    readonly List<Func<InspectMqttPacketEventArgs, Task>> _messageInspectors = new();
    readonly MqttNetEventLogger _mqttNetEventLogger = new();

    IMqttClient? _mqttClient;
    int _receivedMessagesCount;

    public MqttClientService()
    {
        _mqttNetEventLogger.LogMessagePublished += OnLogMessagePublished;
    }

    public event Func<MqttApplicationMessageReceivedEventArgs, Task> ApplicationMessageReceived
    {
        add => _applicationMessageReceivedEvent.AddHandler(value);
        remove => _applicationMessageReceivedEvent.RemoveHandler(value);
    }

    public event EventHandler<MqttClientDisconnectedEventArgs>? Disconnected;

    public event Action<MqttNetLogMessagePublishedEventArgs>? LogMessagePublished;

    public bool IsConnected => _mqttClient?.IsConnected == true;

    public int ReceivedMessagesCount => _receivedMessagesCount;

    public async Task<MqttClientConnectResult> Connect(ConnectionItemViewModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        if (_mqttClient != null)
        {
            _mqttClient.ApplicationMessageReceivedAsync -= OnApplicationMessageReceived;
            _mqttClient.DisconnectedAsync -= OnDisconnected;
            _mqttClient.InspectPacketAsync -= OnInspectPacket;

            await _mqttClient.DisconnectAsync();
            _mqttClient.Dispose();
        }

        _mqttClient = new MqttClientFactory(_mqttNetEventLogger).CreateMqttClient();

        var clientOptionsBuilder = new MqttClientOptionsBuilder().WithTimeout(TimeSpan.FromSeconds(item.ServerOptions.CommunicationTimeout))
            .WithProtocolVersion(item.ServerOptions.SelectedProtocolVersion.Value)
            .WithClientId(item.SessionOptions.ClientId)
            .WithCleanSession(item.SessionOptions.CleanSession)
            .WithCredentials(item.SessionOptions.UserName, item.SessionOptions.Password)
            .WithRequestProblemInformation(item.SessionOptions.RequestProblemInformation)
            .WithRequestResponseInformation(item.SessionOptions.RequestResponseInformation)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(item.SessionOptions.KeepAliveInterval))
            .WithoutPacketFragmentation(); // We do not need this optimization is this type of client. It will also increase compatibility.

        if (item.SessionOptions.SessionExpiryInterval > 0)
        {
            clientOptionsBuilder.WithSessionExpiryInterval((uint)item.SessionOptions.SessionExpiryInterval);
        }

        if (!string.IsNullOrEmpty(item.SessionOptions.AuthenticationMethod))
        {
            clientOptionsBuilder.WithEnhancedAuthentication(item.SessionOptions.AuthenticationMethod, Convert.FromBase64String(item.SessionOptions.AuthenticationData));
        }

        if (item.ServerOptions.SelectedTransport.Value == Transport.TCP)
        {
            clientOptionsBuilder.WithTcpServer(item.ServerOptions.Host, item.ServerOptions.Port);
        }
        else
        {
            clientOptionsBuilder.WithWebSocketServer(o =>
            {
                o.WithUri(item.ServerOptions.Host);
            });
        }

        if (item.ServerOptions.SelectedTlsVersion.Value != SslProtocols.None)
        {
            clientOptionsBuilder.WithTlsOptions(o =>
            {
                o.WithSslProtocols(item.ServerOptions.SelectedTlsVersion.Value);
                o.WithIgnoreCertificateChainErrors(item.ServerOptions.IgnoreCertificateErrors);
                o.WithIgnoreCertificateRevocationErrors(item.ServerOptions.IgnoreCertificateErrors);
                o.WithCertificateValidationHandler(item.ServerOptions.IgnoreCertificateErrors ? _ => true : null);

                if (!string.IsNullOrEmpty(item.SessionOptions.CertificatePath))
                {
                    X509Certificate2Collection certificates = new();

                    if (string.IsNullOrEmpty(item.SessionOptions.CertificatePassword))
                    {
                        certificates.Add(X509CertificateLoader.LoadCertificateFromFile(item.SessionOptions.CertificatePath));
                    }
                    else
                    {
                        certificates.Add(X509CertificateLoader.LoadPkcs12FromFile(item.SessionOptions.CertificatePath, item.SessionOptions.CertificatePassword));
                    }

                    o.WithClientCertificates(certificates);
                    o.WithApplicationProtocols([new SslApplicationProtocol("mqtt")]);
                }
            });
        }

        _mqttClient.ApplicationMessageReceivedAsync += OnApplicationMessageReceived;

        // TODO: Attach and detach packet inspection on demand (internal overhead in MQTTnet library)!
        _mqttClient.InspectPacketAsync += OnInspectPacket;
        _mqttClient.DisconnectedAsync += OnDisconnected;

        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(item.ServerOptions.CommunicationTimeout));
        try
        {
            return await _mqttClient.ConnectAsync(clientOptionsBuilder.Build(), timeout.Token);
        }
        catch (OperationCanceledException)
        {
            if (timeout.IsCancellationRequested)
            {
                throw new MqttCommunicationTimedOutException();
            }

            throw;
        }
    }

    public Task Disconnect()
    {
        ThrowIfNotConnected();

        return _mqttClient.DisconnectAsync();
    }

    public Task<MqttClientPublishResult> Publish(MqttApplicationMessage message, CancellationToken cancellationToken)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        ThrowIfNotConnected();

        return _mqttClient!.PublishAsync(message, cancellationToken);
    }

    public Task<MqttClientPublishResult> Publish(PublishItemViewModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        ThrowIfNotConnected();

        byte[] payload;
        if (item.PayloadFormat == BufferFormat.Plain)
        {
            payload = Encoding.UTF8.GetBytes(item.Payload);
        }
        else if (item.PayloadFormat == BufferFormat.Base64)
        {
            payload = Convert.FromBase64String(item.Payload);
        }
        else if (item.PayloadFormat == BufferFormat.Path)
        {
            payload = File.ReadAllBytes(item.Payload);
        }
        else
        {
            throw new NotSupportedException();
        }

        var applicationMessageBuilder = new MqttApplicationMessageBuilder().WithTopic(item.Topic)
            .WithQualityOfServiceLevel(item.QualityOfServiceLevel.Value)
            .WithRetainFlag(item.Retain)
            .WithMessageExpiryInterval(item.MessageExpiryInterval)
            .WithContentType(item.ContentType)
            .WithPayloadFormatIndicator(item.PayloadFormatIndicator.Value)
            .WithPayload(payload)
            .WithResponseTopic(item.ResponseTopic);

        if (item.SubscriptionIdentifier > 0)
        {
            applicationMessageBuilder.WithSubscriptionIdentifier(item.SubscriptionIdentifier);
        }

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

        return _mqttClient!.PublishAsync(applicationMessageBuilder.Build());
    }

    public void RegisterMessageInspectorHandler(Func<InspectMqttPacketEventArgs, Task> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        _messageInspectors.Add(handler);
    }

    public async Task<MqttClientSubscribeResult> Subscribe(SubscriptionItemViewModel subscriptionItem)
    {
        if (subscriptionItem == null)
        {
            throw new ArgumentNullException(nameof(subscriptionItem));
        }

        ThrowIfNotConnected();

        var topicFilter = new MqttTopicFilterBuilder().WithTopic(subscriptionItem.Topic)
            .WithQualityOfServiceLevel(subscriptionItem.QualityOfServiceLevel.Value)
            .WithNoLocal(subscriptionItem.NoLocal)
            .WithRetainHandling(subscriptionItem.RetainHandling.Value)
            .WithRetainAsPublished(subscriptionItem.RetainAsPublished)
            .Build();

        var subscribeOptionsBuilder = new MqttClientSubscribeOptionsBuilder().WithTopicFilter(topicFilter);

        foreach (var userProperty in subscriptionItem.UserProperties.Items)
        {
            if (!string.IsNullOrEmpty(userProperty.Name))
            {
                subscribeOptionsBuilder.WithUserProperty(userProperty.Name, userProperty.Value);
            }
        }

        var subscribeOptions = subscribeOptionsBuilder.Build();

        return await _mqttClient!.SubscribeAsync(subscribeOptions).ConfigureAwait(false);
    }

    public async Task<MqttClientUnsubscribeResult> Unsubscribe(SubscriptionItemViewModel subscriptionItem)
    {
        if (subscriptionItem == null)
        {
            throw new ArgumentNullException(nameof(subscriptionItem));
        }

        ThrowIfNotConnected();

        return await _mqttClient.UnsubscribeAsync(subscriptionItem.Topic).ConfigureAwait(false);
    }

    async Task OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        Interlocked.Increment(ref _receivedMessagesCount);

        // We have to insert a small delay here because this is an UI application. If we
        // have no delay the application will freeze as soon as there is much traffic.
        await Task.Delay(50);
        await Dispatcher.UIThread.InvokeAsync(() =>
            {
            },
            DispatcherPriority.Render);

        await _applicationMessageReceivedEvent.InvokeAsync(eventArgs);
    }

    Task OnDisconnected(MqttClientDisconnectedEventArgs eventArgs)
    {
        Dispatcher.UIThread.Post(() =>
        {
            Disconnected?.Invoke(this, eventArgs);
        });

        return Task.CompletedTask;
    }

    Task OnInspectPacket(InspectMqttPacketEventArgs eventArgs)
    {
        foreach (var messageInspector in _messageInspectors)
        {
            messageInspector.Invoke(eventArgs).GetAwaiter().GetResult();

            // We have to insert a sleep here to make sure that the UI remains responsive.
            Thread.Sleep(25);
        }

        return Task.CompletedTask;
    }

    void OnLogMessagePublished(object? sender, MqttNetLogMessagePublishedEventArgs e)
    {
        LogMessagePublished?.Invoke(e);
    }

    void ThrowIfNotConnected()
    {
        if (_mqttClient == null || !_mqttClient.IsConnected)
        {
            throw new InvalidOperationException("The MQTT client is not connected.");
        }
    }
}