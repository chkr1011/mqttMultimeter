using System;
using System.Buffers;
using System.Text;
using mqttMultimeter.Common;
using mqttMultimeter.Pages.Inflight;
using MQTTnet;

namespace mqttMultimeter.Pages.TopicExplorer;

public sealed class TopicExplorerItemMessageViewModel : BaseViewModel
{
    public TopicExplorerItemMessageViewModel(DateTime timestamp, MqttApplicationMessage applicationMessage, TimeSpan delay)
    {
        if (applicationMessage == null)
        {
            throw new ArgumentNullException(nameof(applicationMessage));
        }

        Timestamp = timestamp;
        PayloadPreview = GeneratePayloadPreview(applicationMessage.Payload);
        PayloadLength = applicationMessage.Payload.Length;
        Retain = applicationMessage.Retain;

        Delay = delay;
        InflightItem = InflightPageItemViewModelFactory.Create(applicationMessage, 0);
    }

    public TimeSpan Delay { get; }

    public InflightPageItemViewModel InflightItem { get; init; }

    public long PayloadLength { get; }

    public string PayloadPreview { get; }

    public bool Retain { get; }

    public DateTime Timestamp { get; }

    static string GeneratePayloadPreview(ReadOnlySequence<byte> payload)
    {
        if (payload.Length == 0)
        {
            return string.Empty;
        }

        try
        {
            var preview = new StringBuilder();
            preview.Append(Encoding.UTF8.GetString(payload));
            preview = preview.Replace("\r", string.Empty);
            preview = preview.Replace("\n", " ");

            // TODO: To settings.
            const int maxPreviewLength = 100;

            if (preview.Length < maxPreviewLength)
            {
                return preview.ToString();
            }

            return preview.ToString(0, maxPreviewLength) + " ...";
        }
        catch
        {
            return "[mqttMultimeter:INVALID_UTF8]";
        }
    }
}