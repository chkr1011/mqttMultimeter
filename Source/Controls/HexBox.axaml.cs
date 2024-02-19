using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using mqttMultimeter.Extensions;

namespace mqttMultimeter.Controls;

public sealed class HexBox : TemplatedControl
{
    public static readonly StyledProperty<byte[]?> ValueProperty = AvaloniaProperty.Register<HexBox, byte[]?>(nameof(Value));

    public static readonly StyledProperty<string?> PreviewProperty = AvaloniaProperty.Register<HexBox, string?>(nameof(Preview));

    public static readonly StyledProperty<int> CaretIndexProperty = AvaloniaProperty.Register<HexBox, int>(nameof(CaretIndex));

    TextBox? _contentTextBox;


    public int CaretIndex
    {
        get => GetValue(CaretIndexProperty);
        set => SetValue(CaretIndexProperty, value);
    }

    public string? Preview
    {
        get => GetValue(PreviewProperty);
        set => SetValue(PreviewProperty, value);
    }

    public byte[]? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _contentTextBox = (TextBox)this.GetTemplateChild("PART_ContentTextBox");

        Dump();
        UpdateValues();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty)
        {
            Dump();
        }

        if (change.Property == CaretIndexProperty)
        {
            UpdateValues();
        }
    }

    void Dump()
    {
        if (_contentTextBox == null)
        {
            return;
        }

        var buffer = Value ?? Array.Empty<byte>();

        var column = 0;
        var contentBuilder = new StringBuilder(buffer.Length * 3);
        var previewBuilder = new StringBuilder();

        var isFirst = true;
        for (var i = 0; i < buffer.Length; i++)
        {
            var @byte = buffer[i];
            var byteHex = BitConverter.ToString(buffer, i, 1);

            if (!isFirst)
            {
                contentBuilder.Append(' ');
            }

            contentBuilder.Append(byteHex);

            if (@byte is > 0x0020 and < 0x007F)
            {
                previewBuilder.Append((char)@byte);
            }
            else
            {
                previewBuilder.Append('.');
            }

            previewBuilder.Append(' ');

            column++;

            isFirst = false;

            if (column == 16)
            {
                contentBuilder.AppendLine();
                column = 0;

                previewBuilder.AppendLine();

                isFirst = true;
            }
        }

        _contentTextBox.Text = contentBuilder.ToString();
        Preview = previewBuilder.ToString();

        UpdateValues();
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
                stringBuilder.Append('.');
                counter = 0;
            }
        }

        return stringBuilder.ToString();
    }

    void SetValue(string textBoxName, byte[] buffer, int bufferMinLength, Func<byte[], string> valueProvider)
    {
        var textBox = (TextBox)this.GetTemplateChild(textBoxName);

        if (buffer.Length < bufferMinLength)
        {
            textBox.Text = string.Empty;
        }
        else
        {
            textBox.Text = valueProvider(buffer);
        }
    }

    void SetValue(string textBoxName, object value)
    {
        var textBox = (TextBox)this.GetTemplateChild(textBoxName);
        textBox.Text = Convert.ToString(value);
    }

    void UpdateValues()
    {
        var lineBreaks = Regex.Matches(_contentTextBox?.Text?[..CaretIndex] ?? string.Empty, Environment.NewLine).Count;

        var offset = (CaretIndex - lineBreaks) / 3;
        var length = Value?.Length ?? 0;
        var remaining = length - offset - 1;
        if (remaining < 0)
        {
            remaining = 0;
        }

        var buffer = Value?.Skip(offset).ToArray() ?? Array.Empty<byte>();

        SetValue("Length", length.ToString(CultureInfo.InvariantCulture));
        SetValue("ValueOffset", offset.ToString(CultureInfo.InvariantCulture));
        SetValue("ValueRemaining", remaining.ToString(CultureInfo.InvariantCulture));
        SetValue("ValueHex", buffer, 1, b => BitConverter.ToString(b, 0, 1));
        SetValue("Value8Bit", buffer, 1, b => GetBits(b[0]));
        SetValue("ValueBoolean", buffer, 1, b => (b[0] > 0).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueInt8", buffer, 1, b => b[0].ToString(CultureInfo.InvariantCulture));
        SetValue("ValueInt16", buffer, 2, b => BitConverter.ToInt16(b).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueUInt16", buffer, 2, b => BitConverter.ToUInt16(b).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueInt32", buffer, 4, b => BitConverter.ToInt32(b).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueUInt32", buffer, 4, b => BitConverter.ToUInt32(b).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueInt64", buffer, 8, b => BitConverter.ToInt64(b).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueUInt64", buffer, 8, b => BitConverter.ToUInt64(b).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueDouble", buffer, 8, b => BitConverter.ToDouble(b).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueFloat", buffer, 4, b => BitConverter.ToSingle(b).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueAscii", buffer, 1, b => Encoding.ASCII.GetString(b, 0, 1).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueUTF8", buffer, 2, b => Encoding.UTF8.GetString(b, 0, 2).ToString(CultureInfo.InvariantCulture));
        SetValue("ValueUTF32", buffer, 4, b => Encoding.UTF32.GetString(b, 0, 4).ToString(CultureInfo.InvariantCulture));
    }
}