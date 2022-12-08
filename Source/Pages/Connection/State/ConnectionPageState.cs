using System.Collections.Generic;

namespace mqttMultimeter.Pages.Connection.State;

public sealed class ConnectionPageState
{
    public const string Key = "Connections";

    public List<ConnectionState> Connections { get; set; } = new();
}