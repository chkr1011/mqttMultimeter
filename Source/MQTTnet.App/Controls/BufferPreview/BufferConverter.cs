using System;

namespace MQTTnet.App.Controls;

public sealed class BufferConverter
{
    public string? Caption { get; set; }

    public Func<byte[], string>? Convert { get; set; }
}