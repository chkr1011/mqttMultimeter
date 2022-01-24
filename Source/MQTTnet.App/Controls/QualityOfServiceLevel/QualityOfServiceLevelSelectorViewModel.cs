using System;
using MQTTnet.App.Common;
using MQTTnet.Protocol;

namespace MQTTnet.App.Controls.QualityOfServiceLevel;

public sealed class QualityOfServiceLevelSelectorViewModel : BaseViewModel
{
    public QualityOfServiceLevelSelectorViewModel()
    {
        Value = MqttQualityOfServiceLevel.AtMostOnce;
    }

    public bool IsLevel0
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsLevel1
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsLevel2
    {
        get => GetValue<bool>();
        set => SetValue(value);
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