using System.Collections.Generic;

namespace MQTTnetApp.Pages.Connection.State;

public sealed class ConnectionPageState
{
    public List<ConnectionState> Connections { get; set; } = new();
}