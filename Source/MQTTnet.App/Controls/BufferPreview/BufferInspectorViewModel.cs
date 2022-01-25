using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MQTTnet.App.Common;

namespace MQTTnet.App.Controls;

public sealed class BufferInspectorViewModel : BaseViewModel
{
    readonly BufferValueViewModel _asciiCharValue = new("ASCII char");
    readonly BufferValueViewModel _bitsValue = new("Bits");
    readonly BufferValueViewModel _booleanValue = new("Boolean");
    readonly BufferValueViewModel _bytesLeftValue = new("Bytes left");

    readonly BufferValueViewModel _byteValue = new("Byte");
    readonly BufferValueViewModel _doubleValue = new("Double");
    readonly BufferValueViewModel _floatValue = new("Float");

    readonly BufferValueViewModel _int16Value = new("Int 16");

    readonly BufferValueViewModel _int32Value = new("Int 32");

    readonly BufferValueViewModel _int64Value = new("Int 64");
    readonly BufferValueViewModel _offsetValue = new("Offset");
    readonly BufferValueViewModel _uint16Value = new("UInt 16");
    readonly BufferValueViewModel _uint32Value = new("UInt 32");
    readonly BufferValueViewModel _uint64Value = new("UInt 64");
    readonly BufferValueViewModel _unicodeCharValue = new("Unicode char");
    readonly BufferValueViewModel _utf8Char = new("UTF8 char");
    string _base64Content;

    byte[] _buffer;

    int _hexCaretIndex;
    string _hexContent;
    string _hexPreview;
    string _jsonContent;
    string _utf8Content;

    public BufferInspectorViewModel()
    {
        Values.Add(_offsetValue);
        Values.Add(_bytesLeftValue);

        Values.Add(_byteValue);
        Values.Add(_booleanValue);
        Values.Add(_bitsValue);
        Values.Add(_asciiCharValue);

        Values.Add(_int16Value);
        Values.Add(_uint16Value);
        Values.Add(_utf8Char);

        Values.Add(_int32Value);
        Values.Add(_uint32Value);
        Values.Add(_floatValue);
        Values.Add(_int64Value);
        Values.Add(_uint64Value);
        Values.Add(_doubleValue);
        Values.Add(_unicodeCharValue);
    }

    public byte[] Buffer
    {
        get => _buffer;
        private set
        {
            _buffer = value;
            OnPropertyChanged();
        }
    }

    public int HexCaretIndex
    {
        get => _hexCaretIndex;
        set
        {
            _hexCaretIndex = value;

            TryParseValuesFromHexOffset();
        }
    }

    public string HexContent
    {
        get => _hexContent;
        set
        {
            _hexContent = value;
            OnPropertyChanged();
        }
    }

    public string HexPreview
    {
        get => _hexPreview;
        set
        {
            _hexPreview = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<BufferValueViewModel> Values { get; } = new();

    public void Dump(byte[] buffer)
    {
        Buffer = buffer;

        DumpAsHex(buffer);

        HexCaretIndex = 0;
    }

    public static byte[] StringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
    }

    void DumpAsHex(byte[] buffer)
    {
        var column = 0;
        var contentBuilder = new StringBuilder();
        var previewBuilder = new StringBuilder();

        foreach (var @byte in buffer)
        {
            var byteHex = BitConverter.ToString(new[]
            {
                @byte
            });

            contentBuilder.Append(byteHex);
            contentBuilder.Append(' ');

            if (@byte > 0x0020 && @byte < 0x007F)
            {
                previewBuilder.Append((char)@byte);
            }
            else
            {
                previewBuilder.Append('.');
            }

            column++;

            if (column == 16)
            {
                contentBuilder.AppendLine();
                column = 0;

                previewBuilder.AppendLine();
            }
        }

        HexContent = contentBuilder.ToString();
        HexPreview = previewBuilder.ToString();
    }

    static string GetBits(byte @byte)
    {
        var stringBuilder = new StringBuilder();
        var counter = 0;
        for (var i = 7; i >= 0; i--)
        {
            stringBuilder.Append((@byte & (1 << i)) > 0 ? "1" : "0");

            counter++;
            if (counter == 4 && i != 0)
            {
                stringBuilder.Append(".");
                counter = 0;
            }
        }

        return stringBuilder.ToString();
    }

    void TryParseValuesFromHexOffset()
    {
        var hexContent = HexContent;

        var i = _hexCaretIndex;
        if (i + 1 > hexContent.Length - 1)
        {
            return;
        }

        if (hexContent[i + 1] == ' ')
        {
            i--;
        }

        var source = hexContent.Substring(i);
        source = source.Replace(Environment.NewLine, " ");
        source = source.Trim();
        source = source.Replace(" ", "");

        var buffer = StringToByteArray(source);

        foreach (var value in Values)
        {
            value.SetValue(string.Empty);
        }

        _offsetValue.SetValue(_buffer.Length - buffer.Length);
        _bytesLeftValue.SetValue(buffer.Length);

        _byteValue.SetValue(buffer[0]);
        _booleanValue.SetValue(buffer[0] > 0);
        _bitsValue.SetValue(GetBits(buffer[0]));
        _asciiCharValue.SetValue((char)buffer[0]);

        if (buffer.Length < 2)
        {
            return;
        }

        _int16Value.SetValue(BitConverter.ToInt16(buffer));
        _uint16Value.SetValue(BitConverter.ToUInt16(buffer));
        _utf8Char.SetValue(Encoding.UTF8.GetString(buffer, 0, 2));

        if (buffer.Length < 4)
        {
            return;
        }

        _int32Value.SetValue(BitConverter.ToInt32(buffer));
        _uint32Value.SetValue(BitConverter.ToUInt32(buffer));
        _floatValue.SetValue(BitConverter.ToSingle(buffer));

        if (buffer.Length < 8)
        {
            return;
        }

        _int64Value.SetValue(BitConverter.ToInt64(buffer));
        _uint64Value.SetValue(BitConverter.ToUInt64(buffer));
        _doubleValue.SetValue(BitConverter.ToDouble(buffer));
        _unicodeCharValue.SetValue(Encoding.Unicode.GetString(buffer, 0, 4));
    }
}