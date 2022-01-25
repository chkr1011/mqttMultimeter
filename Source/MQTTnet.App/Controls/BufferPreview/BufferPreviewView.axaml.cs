using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MQTTnet.App.Extensions;
using MQTTnet.App.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MQTTnet.App.Controls;

public sealed class BufferInspectorView : TemplatedControl
{
    public static readonly StyledProperty<byte[]?> BufferProperty = AvaloniaProperty.Register<BufferInspectorView, byte[]?>(nameof(Buffer));

    public static readonly StyledProperty<BufferConverter> SelectedFormatProperty = AvaloniaProperty.Register<BufferInspectorView, BufferConverter>(nameof(SelectedFormat));

    public static readonly StyledProperty<IList<BufferConverter>> FormatsProperty = AvaloniaProperty.Register<BufferInspectorView, IList<BufferConverter>>(nameof(Formats));

    public static readonly StyledProperty<bool> ShowRawProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(ShowRaw));

    TextBox? _stringContent;

    public BufferInspectorView()
    {
        Formats = new ObservableCollection<BufferConverter>();

        Formats.Add(new BufferConverter
        {
            Caption = "ASCII",
            Convert = b => Encoding.ASCII.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Caption = "Base64",
            Convert = Convert.ToBase64String
        });

        Formats.Add(new BufferConverter
        {
            Caption = "Binary",
            Convert = BinaryEncoder.GetString
        });

        Formats.Add(new BufferConverter
        {
            Caption = "HEX",
            Convert = HexEncoder.GetString
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

        Formats.Add(new BufferConverter
        {
            Caption = "RAW",
            Convert = null // Special case!
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
            Caption = "Unicode",
            Convert = b => Encoding.Unicode.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Caption = "UTF-8",
            Convert = b => Encoding.UTF8.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Caption = "UTF-32",
            Convert = b => Encoding.UTF32.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Caption = "XML",
            Convert = b =>
            {
                var xml = Encoding.UTF8.GetString(b);
                return XDocument.Parse(xml).ToString(SaveOptions.None);
            }
        });

        SelectedFormat = Formats.First(s => s.Caption == "UTF-8");
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

    public bool ShowRaw
    {
        get => GetValue(ShowRawProperty);
        set => SetValue(ShowRawProperty, value);
    }

    // TODO: Fix
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
        
        if (change.Property == SelectedFormatProperty)
        {
            ShowRaw = SelectedFormat.Caption == "RAW";
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

        if (buffer.Length == 0)
        {
            _stringContent.Text = string.Empty;
            return;
        }

        var format = SelectedFormat;
        if (format == null)
        {
            throw new InvalidOperationException();
        }

        try
        {
            _stringContent.Text = format.Convert?.Invoke(buffer);
        }
        catch (Exception exception)
        {
            _stringContent.Text = $"<{exception.Message}>";
        }
    }
}