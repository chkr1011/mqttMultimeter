using System;
using MQTTnet.App.Common;
using MQTTnet.Protocol;
using ReactiveUI;

namespace MQTTnet.App.Controls;

public sealed class QualityOfServiceLevelSelectorViewModel : BaseViewModel
{
    bool _isLevel0 = true;
    bool _isLevel1;
    bool _isLevel2;

    public QualityOfServiceLevelSelectorViewModel()
    {
        Value = MqttQualityOfServiceLevel.AtMostOnce;
    }

    public bool IsLevel0
    {
        get => _isLevel0;
        set => this.RaiseAndSetIfChanged(ref _isLevel0, value);
    }

    public bool IsLevel1
    {
        get => _isLevel1;
        set => this.RaiseAndSetIfChanged(ref _isLevel1, value);
    }

    public bool IsLevel2
    {
        get => _isLevel2;
        set => this.RaiseAndSetIfChanged(ref _isLevel2, value);
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