﻿using MQTTnet.App.Common;
using MQTTnet.Client;
using ReactiveUI;

namespace MQTTnet.App.Pages.Subscriptions;

public sealed class SubscriptionResponseViewModel : BaseViewModel
{
    string _reasonCodeText = string.Empty;
    string _reasonString = string.Empty;
    int? _reasonCode;

    public string ReasonCodeText
    {
        get => _reasonCodeText;
        set => this.RaiseAndSetIfChanged(ref _reasonCodeText, value);
    }
    
    public string ReasonString
    {
        get => _reasonString;
        set => this.RaiseAndSetIfChanged(ref _reasonString, value);
    }

    public int? ReasonCode
    {
        get => _reasonCode;
        set => this.RaiseAndSetIfChanged(ref _reasonCode, value);
    }

    public void ApplyResponse(MqttClientSubscribeResult response)
    {
        ReasonCodeText = response.Items[0].ResultCode.ToString();
        ReasonCode = (int)response.Items[0].ResultCode;
        ReasonString = response.ReasonString;
        // TODO: Import Packet Identifier
    }

    public void ApplyResponse(MqttClientUnsubscribeResult response)
    {
        ReasonCodeText = response.Items[0].ResultCode.ToString();
        ReasonCode = (int)response.Items[0].ResultCode;
        ReasonString = response.ReasonString;
        // TODO: Import Packet Identifier
    }
}