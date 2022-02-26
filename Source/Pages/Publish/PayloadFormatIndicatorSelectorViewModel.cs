using System;
using MQTTnet.Protocol;
using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.Publish;

public sealed class PayloadFormatIndicatorSelectorViewModel : BaseViewModel
{
    bool _isCharacterData;
    bool _isUnspecified = true;

    public bool IsCharacterData
    {
        get => _isCharacterData;
        set => this.RaiseAndSetIfChanged(ref _isCharacterData, value);
    }

    public bool IsUnspecified
    {
        get => _isUnspecified;
        set => this.RaiseAndSetIfChanged(ref _isUnspecified, value);
    }

    public MqttPayloadFormatIndicator Value
    {
        get
        {
            if (IsCharacterData)
            {
                return MqttPayloadFormatIndicator.CharacterData;
            }

            return MqttPayloadFormatIndicator.Unspecified;
        }

        set
        {
            switch (value)
            {
                case MqttPayloadFormatIndicator.Unspecified:
                {
                    IsUnspecified = true;
                    IsCharacterData = false;
                    break;
                }

                case MqttPayloadFormatIndicator.CharacterData:
                {
                    IsUnspecified = false;
                    IsCharacterData = true;
                    break;
                }

                default:
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}