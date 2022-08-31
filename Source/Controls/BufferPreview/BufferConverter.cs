using System;

namespace MQTTnetApp.Controls;

public sealed class BufferConverter
{
    public Func<byte[], string>? Convert { get; set; }

    public string? Name { get; set; }
    
    public string? LanguageExtension { get; set; }
}