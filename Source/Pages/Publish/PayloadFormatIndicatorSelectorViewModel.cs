using System;
using mqttMultimeter.Common;
using MQTTnet.Protocol;

namespace mqttMultimeter.Pages.Publish;

public sealed class PayloadFormatIndicatorSelectorViewModel : BaseSingleSelectionViewModel
{
    const int CharacterDataIndex = 0;
    const int UnspecifiedIndex = 1;

    public PayloadFormatIndicatorSelectorViewModel() : base(2)
    {
        IsUnspecified = true;
    }

    public bool IsCharacterData
    {
        get => GetState(CharacterDataIndex);
        set => UpdateStates(CharacterDataIndex, value);
    }

    public bool IsUnspecified
    {
        get => GetState(UnspecifiedIndex);
        set => UpdateStates(UnspecifiedIndex, value);
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