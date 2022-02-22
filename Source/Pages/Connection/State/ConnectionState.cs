namespace MQTTnetApp.Pages.Connection.State;

public sealed class ConnectionState
{
    public string? ClientId { get; set; }
    
    public string? Host { get; set; }

    public string? Name { get; set; }

    public int Port { get; set; }

    public string? UserName { get; set; }
}