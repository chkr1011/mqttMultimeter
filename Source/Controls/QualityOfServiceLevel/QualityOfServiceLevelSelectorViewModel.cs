using System;
using mqttMultimeter.Common;
using MQTTnet.Protocol;

namespace mqttMultimeter.Controls;

public sealed class QualityOfServiceLevelSelectorViewModel : BaseSingleSelectionViewModel
{
    public QualityOfServiceLevelSelectorViewModel() : base(3)
    {
        Value = MqttQualityOfServiceLevel.AtMostOnce;
    }

    public bool IsLevel0
    {
        get => GetState(0);
        set => UpdateStates(0, value);
    }

    public bool IsLevel1
    {
        get => GetState(1);
        set => UpdateStates(1, value);
    }

    public bool IsLevel2
    {
        get => GetState(2);
        set => UpdateStates(2, value);
    }

    public MqttQualityOfServiceLevel Value
    {
        get
        {
            if (IsLevel0)
            {
                return MqttQualityOfServiceLevel.AtMostOnce;
            }

            if (IsLevel1)
            {
                return MqttQualityOfServiceLevel.AtLeastOnce;
            }

            if (IsLevel2)
            {
                return MqttQualityOfServiceLevel.ExactlyOnce;
            }

            throw new NotSupportedException();
        }

        set
        {
            IsLevel0 = value == MqttQualityOfServiceLevel.AtMostOnce;
            IsLevel1 = value == MqttQualityOfServiceLevel.AtLeastOnce;
            IsLevel2 = value == MqttQualityOfServiceLevel.ExactlyOnce;
        }
    }
}