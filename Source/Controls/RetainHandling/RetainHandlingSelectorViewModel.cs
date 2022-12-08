using System;
using mqttMultimeter.Common;
using MQTTnet.Protocol;

namespace mqttMultimeter.Controls;

public sealed class RetainHandlingSelectorViewModel : BaseSingleSelectionViewModel
{
    const int DoNotSendOnSubscribeIndex = 0;
    const int SendAtSubscribeIndex = 1;
    const int SendAtSubscribeIfNewSubscriptionOnly = 2;

    public RetainHandlingSelectorViewModel() : base(3)
    {
        Value = MqttRetainHandling.SendAtSubscribe;
    }

    public bool IsDoNotSendOnSubscribe
    {
        get => GetState(DoNotSendOnSubscribeIndex);
        set => UpdateStates(DoNotSendOnSubscribeIndex, value);
    }

    public bool IsSendAtSubscribe
    {
        get => GetState(SendAtSubscribeIndex);
        set => UpdateStates(SendAtSubscribeIndex, value);
    }

    public bool IsSendAtSubscribeIfNewSubscriptionOnly
    {
        get => GetState(SendAtSubscribeIfNewSubscriptionOnly);
        set => UpdateStates(SendAtSubscribeIfNewSubscriptionOnly, value);
    }

    public MqttRetainHandling Value
    {
        get
        {
            if (IsSendAtSubscribe)
            {
                return MqttRetainHandling.SendAtSubscribe;
            }

            if (IsSendAtSubscribeIfNewSubscriptionOnly)
            {
                return MqttRetainHandling.SendAtSubscribeIfNewSubscriptionOnly;
            }

            if (IsDoNotSendOnSubscribe)
            {
                return MqttRetainHandling.DoNotSendOnSubscribe;
            }

            throw new NotSupportedException();
        }

        set
        {
            IsSendAtSubscribe = value == MqttRetainHandling.SendAtSubscribe;
            IsDoNotSendOnSubscribe = value == MqttRetainHandling.DoNotSendOnSubscribe;
            IsSendAtSubscribeIfNewSubscriptionOnly = value == MqttRetainHandling.SendAtSubscribeIfNewSubscriptionOnly;
        }
    }
}