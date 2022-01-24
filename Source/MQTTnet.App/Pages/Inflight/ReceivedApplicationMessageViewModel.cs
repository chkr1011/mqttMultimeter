using System;

namespace MQTTnet.App.Pages.Inflight;

public sealed class ReceivedApplicationMessageViewModel
{
    public DateTime Timestamp { get; set; }
    
    public int Number { get; set; }
    
    public string Topic { get; set; }
    
    public long Length { get; set; }
    
    public MqttApplicationMessage Source { get; set; }
}