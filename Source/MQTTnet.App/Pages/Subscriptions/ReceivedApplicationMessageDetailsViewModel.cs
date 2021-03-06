using System;
using MQTTnet.App.Common;
using MQTTnet.App.Common.BufferInspector;

namespace MQTTnet.App.Pages.Subscriptions
{
    public sealed class ReceivedApplicationMessageDetailsViewModel : BaseViewModel
    {
        public ReceivedApplicationMessageDetailsViewModel(MqttApplicationMessage message)
        {
            Timestamp = DateTime.Now;
            Topic = message.Topic;
            PayloadLength = message.Payload?.Length ?? 0;
            QualityOfServiceLevel = $"{(int)message.QualityOfServiceLevel} ({message.QualityOfServiceLevel})";

            PayloadInspector = new BufferInspectorViewModel();
            PayloadInspector.Dump(message.Payload ?? Array.Empty<byte>());
        }

        public DateTime Timestamp { get; }

        public string Topic
        {
            get;
        }

        public int PayloadLength
        {
            get;
        }

        public string QualityOfServiceLevel
        {
            get;
        }

        public BufferInspectorViewModel PayloadInspector { get; }
    }
}