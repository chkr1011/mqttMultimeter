namespace MQTTnetApp.Pages.Log;

public sealed class LogItemViewModel
{
    public string Level { get; set; }
    
    public string Timestamp { get; set; }
    
    public string Source { get; set; }
    
    public string Message { get; set; }
}