using System;

namespace mqttMultimeter.Controls;

public sealed class BufferConverter(string name, string? grammar, Func<byte[], string> convert)
{
    readonly Func<byte[], string> _convertCallback = convert ?? throw new ArgumentNullException(nameof(convert));

    public string? Grammar { get; } = grammar;

    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

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