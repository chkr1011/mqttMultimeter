using System;
using MQTTnet.Protocol;

namespace MQTTnet.App.Common.QualityOfServiceLevel
{
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
                IsLevel0 = false;
                IsLevel1 = false;
                IsLevel2 = false;

                if (value == MqttQualityOfServiceLevel.AtMostOnce)
                {
                    IsLevel0 = true;
                }

                if (value == MqttQualityOfServiceLevel.AtLeastOnce)
                {
                    IsLevel1 = true;
                }

                if (value == MqttQualityOfServiceLevel.ExactlyOnce)
                {
                    IsLevel2 = true;
                }
            }
        }
    }
}
