using System;
using System.IO;
using System.Text;
using Avalonia.Threading;

namespace MQTTnetApp.Services.Scripting;

public sealed class PythonTextOutputStream : Stream
{
    public event Action<string>? OutputWritten;

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => true;

    public override long Length => 0;

    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush()
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (count == 0)
        {
            return;
        }

        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        var text = Encoding.UTF8.GetString(buffer, offset, count);

        Dispatcher.UIThread.Post(() =>
        {
            OutputWritten?.Invoke(text);
        });
    }
}