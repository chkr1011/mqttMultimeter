using System;

namespace MQTTnetApp.Controls;

public sealed class BufferConverter
{
    readonly Func<byte[], string> _convertCallback;

    public BufferConverter(string name, string? grammar, Func<byte[], string> convert)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Grammar = grammar;
        _convertCallback = convert ?? throw new ArgumentNullException(nameof(convert));
    }

    public string? Grammar { get; }

    public string Name { get; }

    public string Convert(byte[]? buffer)
    {
        if (buffer == null)
        {
            return string.Empty;
        }

        if (buffer.Length == 0)
        {
            return string.Empty;
        }

        return _convertCallback.Invoke(buffer);
    }
}