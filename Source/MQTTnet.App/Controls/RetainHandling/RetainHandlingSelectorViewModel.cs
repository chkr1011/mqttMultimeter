using System;
using MQTTnet.App.Common;
using MQTTnet.Protocol;
using ReactiveUI;

namespace MQTTnet.App.Controls;

public sealed class RetainHandlingSelectorViewModel : BaseViewModel
{
    bool _doNotSendOnSubscribe;
    bool _isSendAtSubscribe;
    bool _sendAtSubscribeIfNewSubscriptionOnly;

    public RetainHandlingSelectorViewModel()
    {
        RetainHandling = MqttRetainHandling.SendAtSubscribe;
    }

    public bool IsDoNotSendOnSubscribe
    {
        get => _doNotSendOnSubscribe;
        set => this.RaiseAndSetIfChanged(ref _doNotSendOnSubscribe, value);
    }

    public bool IsSendAtSubscribe
    {
        get => _isSendAtSubscribe;
        set => this.RaiseAndSetIfChanged(ref _isSendAtSubscribe, value);
    }

    public bool IsSendAtSubscribeIfNewSubscriptionOnly
    {
        get => _sendAtSubscribeIfNewSubscriptionOnly;
        set => this.RaiseAndSetIfChanged(ref _sendAtSubscribeIfNewSubscriptionOnly, value);
    }

    public MqttRetainHandling RetainHandling
    {
        get
        {
            if (_isSendAtSubscribe)
            {
                return MqttRetainHandling.SendAtSubscribe;
            }

            if (_sendAtSubscribeIfNewSubscriptionOnly)
            {
                return MqttRetainHandling.SendAtSubscribeIfNewSubscriptionOnly;
            }

            if (_doNotSendOnSubscribe)
            {
                return MqttRetainHandling.DoNotSendOnSubscribe;
            }

            throw new NotSupportedException();
        }

        set
        {
            _isSendAtSubscribe = value == MqttRetainHandling.SendAtSubscribe;
            _doNotSendOnSubscribe = value == MqttRetainHandling.DoNotSendOnSubscribe;
            _sendAtSubscribeIfNewSubscriptionOnly = value == MqttRetainHandling.SendAtSubscribeIfNewSubscriptionOnly;
        }
    }
}