using System;

namespace MQTTnet.App.Controls;

public sealed class BufferConverter
{
    public Func<byte[], string>? Convert { get; set; }
    public string? Name { get; set; }
}