using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MessagePack;
using MQTTnetApp.Extensions;
using MQTTnetApp.Text;

namespace MQTTnetApp.Controls;

public sealed class BufferInspectorView : TemplatedControl
{
    public static readonly StyledProperty<byte[]?> BufferProperty = AvaloniaProperty.Register<BufferInspectorView, byte[]?>(nameof(Buffer));

    public static readonly StyledProperty<BufferConverter?> SelectedFormatProperty = AvaloniaProperty.Register<BufferInspectorView, BufferConverter?>(nameof(SelectedFormat));

    public static readonly StyledProperty<IList<BufferConverter>> FormatsProperty = AvaloniaProperty.Register<BufferInspectorView, IList<BufferConverter>>(nameof(Formats));

    public static readonly StyledProperty<bool> ShowRawProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(ShowRaw));

    public static readonly StyledProperty<string> PreviewContentProperty = AvaloniaProperty.Register<BufferInspectorView, string>(nameof(PreviewContent));

    public static readonly StyledProperty<string?> SelectedFormatNameProperty = AvaloniaProperty.Register<BufferInspectorView, string?>(nameof(SelectedFormatName), "UTF-8");

    Button? _copyToClipboardButton;

    public BufferInspectorView()
    {
        Formats = new ObservableCollection<BufferConverter>();

        Formats.Add(new BufferConverter
        {
            Name = "ASCII",
            Convert = b => Encoding.ASCII.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Name = "Base64",
            Convert = Convert.ToBase64String
        });

        Formats.Add(new BufferConverter
        {
            Name = "Binary",
            Convert = BinaryEncoder.GetString
        });

        Formats.Add(new BufferConverter
        {
            Name = "HEX",
            Convert = HexEncoder.GetString
        });

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        
        Formats.Add(new BufferConverter
        {
            Name = "JSON",
            Convert = b =>
            {
                var json = Encoding.UTF8.GetString(b);
                return JsonSerializer.Serialize(JsonNode.Parse(json), jsonSerializerOptions);
            }
        });
        
        Formats.Add(new BufferConverter
        {
            Name = "MessagePack",
            Convert = b =>
            {
                var json = MessagePackSerializer.ConvertToJson(b);
                return JToken.Parse(json).ToString(Formatting.Indented);
            }
        });

        Formats.Add(new BufferConverter
        {
            Name = "RAW",
            Convert = null // Special case!
        });

        Formats.Add(new BufferConverter
        {
            Name = "Unicode",
            Convert = b => Encoding.Unicode.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Name = "UTF-8",
            Convert = b => Encoding.UTF8.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Name = "UTF-32",
            Convert = b => Encoding.UTF32.GetString(b)
        });

        Formats.Add(new BufferConverter
        {
            Name = "XML",
            Convert = b =>
            {
                var xml = Encoding.UTF8.GetString(b);
                return XDocument.Parse(xml).ToString(SaveOptions.None);
            }
        });

        SelectFormat();
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

    public string PreviewContent
    {
        get => GetValue(PreviewContentProperty);
        set => SetValue(PreviewContentProperty, value);
    }

    public BufferConverter? SelectedFormat
    {
        get => GetValue(SelectedFormatProperty);
        set => SetValue(SelectedFormatProperty, value);
    }

    public string? SelectedFormatName
    {
        get => GetValue(SelectedFormatNameProperty);
        set => SetValue(SelectedFormatNameProperty, value);
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

        _copyToClipboardButton = (Button)this.GetTemplateChild("CopyToClipboardButton");
        _copyToClipboardButton.Click += OnCopyToClipboard;

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
            ShowRaw = SelectedFormat?.Name == "RAW";
        }

        if (change.Property == SelectedFormatNameProperty)
        {
            SelectFormat();
        }
    }

    void OnCopyToClipboard(object? sender, RoutedEventArgs e)
    {
        var clipboardContent = PreviewContent;

        if (!string.IsNullOrEmpty(clipboardContent))
        {
            Application.Current?.Clipboard?.SetTextAsync(clipboardContent);
        }
    }

    void ReadBuffer()
    {
        var buffer = Buffer;
        if (buffer == null)
        {
            buffer = Array.Empty<byte>();
        }

        if (buffer.Length == 0)
        {
            PreviewContent = string.Empty;
            return;
        }

        var format = SelectedFormat;
        if (format == null)
        {
            throw new InvalidOperationException();
        }

        try
        {
            PreviewContent = format.Convert?.Invoke(buffer) ?? string.Empty;
        }
        catch (Exception exception)
        {
            PreviewContent = $"<{exception.Message}>";
        }
    }

    void SelectFormat()
    {
        if (string.IsNullOrEmpty(SelectedFormatName))
        {
            SelectedFormat = Formats.FirstOrDefault();
        }

        SelectedFormat = Formats.FirstOrDefault(f => string.Equals(f.Name, SelectedFormatName));

        if (SelectedFormat == null)
        {
            SelectedFormat = Formats.FirstOrDefault();
        }
    }
}