using System;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MQTTnet.App.Extensions;

namespace MQTTnet.App.Controls;

public sealed class HexBox : TemplatedControl
{
    public static readonly StyledProperty<byte[]?> ValueProperty = AvaloniaProperty.Register<HexBox, byte[]?>(nameof(Value));

    public static readonly StyledProperty<string?> PreviewProperty = AvaloniaProperty.Register<HexBox, string?>(nameof(Preview));

    TextBox? _contentTextBox;

    public string? Preview
    {
        get => GetValue(PreviewProperty);
        set => SetValue(PreviewProperty, value);
    }
    //TextBox? _previewTextBox;

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
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty)
        {
            Dump();
        }
    }

    void Dump()
    {
        // PART_ContentTextBox


        // var previewTextBox = (TextBlock)this.FindVisualChild("PART_PreviewTextBlock");

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
            var byteHex = BitConverter.ToString(new[]
            {
                @byte
            });

            if (!isFirst)
            {
                contentBuilder.Append(' ');
            }

            contentBuilder.Append(byteHex);

            if (@byte > 0x0020 && @byte < 0x007F)
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
    }
}