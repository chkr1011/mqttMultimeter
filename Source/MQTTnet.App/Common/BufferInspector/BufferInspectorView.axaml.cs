using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MQTTnet.App.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MQTTnet.App.Common.BufferInspector;

public sealed class BufferConverter
{
    public string Caption { get; set; }

    public Func<byte[], string> Convert { get; set; }
}

public sealed class BufferInspectorView : TemplatedControl
{
    public static readonly StyledProperty<byte[]?> BufferProperty = AvaloniaProperty.Register<BufferInspectorView, byte[]?>(nameof(Buffer));

    public static readonly StyledProperty<BufferConverter> SelectedFormatProperty =
        AvaloniaProperty.Register<BufferInspectorView, BufferConverter>(nameof(SelectedFormat));

    public static readonly StyledProperty<IList<BufferConverter>> FormatsProperty =
        AvaloniaProperty.Register<BufferInspectorView, IList<BufferConverter>>(nameof(Formats));

    TextBox? _stringContent;

    // public static readonly StyledProperty<int> CaretIndexProperty = AvaloniaProperty.Register<BufferInspectorView, int>(nameof(Buffer));
    //

    public BufferInspectorView()
    {
        Formats = new ObservableCollection<BufferConverter>();

        Formats.Add(new BufferConverter
        {
            Caption = "UTF8",
            Convert = b => Encoding.UTF8.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Caption = "Base64",
            Convert = Convert.ToBase64String
        });

        Formats.Add(new BufferConverter
        {
            Caption = "JSON",
            Convert = b =>
            {
                var json = Encoding.UTF8.GetString(b);
                return JToken.Parse(json).ToString(Formatting.Indented);
            }
        });

        // Formats.Add(new BufferConverterViewModel
        // {
        //     Caption = "MessagePack",
        //     Convert = b =>
        //     {
        //         MessagePack.Formatters.
        //         var json = Encoding.UTF8.GetString(b);
        //         return JToken.Parse(json).ToString(Formatting.Indented);
        //     }
        // });

        Formats.Add(new BufferConverter
        {
            Caption = "XML",
            Convert = b =>
            {
                var xml = Encoding.UTF8.GetString(b);
                return XDocument.Parse(xml).ToString(SaveOptions.None);
            }
        });

        SelectedFormat = Formats.FirstOrDefault()!;
    }

    public byte[]? Buffer
    {
        get => GetValue(BufferProperty);
        set => SetValue(BufferProperty, value);
    }

    public IList<BufferConverter> Formats
    {
        get => GetValue(FormatsProperty);
        set => SetValue(FormatsProperty, value);
    }

    public BufferConverter SelectedFormat
    {
        get => GetValue(SelectedFormatProperty);
        set => SetValue(SelectedFormatProperty, value);
    }

    public static byte[] TestData => Encoding.UTF8.GetBytes("Hallo");

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _stringContent = (TextBox)this.GetTemplateChild("PART_StringContent")!;

        ReadBuffer();
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == BufferProperty || change.Property == SelectedFormatProperty)
        {
            ReadBuffer();
        }
    }

    void ReadBuffer()
    {
        if (_stringContent == null)
        {
            return;
        }

        var buffer = Buffer;
        if (buffer == null)
        {
            buffer = Array.Empty<byte>();
        }

        try
        {
            _stringContent.Text = SelectedFormat.Convert(buffer);
        }
        catch (Exception exception)
        {
            _stringContent.Text = $"<{exception.Message}>";
        }
    }
}