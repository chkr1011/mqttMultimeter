using System;

namespace MQTTnetApp.Pages.Scripts;

public sealed class ReceiveScriptEventArgs : EventArgs
{
    public string? Script { get; set; }
}