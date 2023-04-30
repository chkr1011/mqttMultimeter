namespace mqttMultimeter.Pages.Log;

public sealed class LogItemViewModel
{
    public string? Exception { get; set; }
    public bool IsError { get; set; }

    public bool IsInformation { get; set; }

    public bool IsVerbose { get; set; }

    public bool IsWarning { get; set; }

    public string? Level { get; set; }

    public string? Message { get; set; }

    public string? Source { get; set; }

    public string? Timestamp { get; set; }
}