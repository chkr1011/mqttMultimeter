using System.Collections.Generic;

namespace MQTTnetApp.Pages.Scripts.State;

public sealed class ScriptsPageState
{
    public const string Key = "Scripts";

    public List<ScriptState> Scripts { get; set; } = new();
}