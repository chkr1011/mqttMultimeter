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
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using AvaloniaEdit.TextMate.Grammars;
using MessagePack;
using MQTTnetApp.Extensions;
using MQTTnetApp.Services.Data;
using MQTTnetApp.Text;

namespace MQTTnetApp.Controls;

public sealed class BufferInspectorView : TemplatedControl
{
    public static readonly StyledProperty<byte[]?> BufferProperty = AvaloniaProperty.Register<BufferInspectorView, byte[]?>(nameof(Buffer));

    public static readonly StyledProperty<BufferConverter?> SelectedFormatProperty = AvaloniaProperty.Register<BufferInspectorView, BufferConverter?>(nameof(SelectedFormat));

    public static readonly StyledProperty<IList<BufferConverter>> FormatsProperty = AvaloniaProperty.Register<BufferInspectorView, IList<BufferConverter>>(nameof(Formats));

    public static readonly StyledProperty<bool> ShowRawProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(ShowRaw));

    public static readonly StyledProperty<string?> SelectedFormatNameProperty = AvaloniaProperty.Register<BufferInspectorView, string?>(nameof(SelectedFormatName), "UTF-8");

    readonly RegistryOptions _textEditorRegistryOptions = new(ThemeName.Dark);

    string _content = string.Empty;
    Button? _copyToClipboardButton;
    string? _currentTextEditorLanguage;
    TextEditor? _textEditor;
    TextMate.Installation? _textMateInstallation;

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
            },
            LanguageExtension = ".jsonc"
        });

        Formats.Add(new BufferConverter
        {
            Name = "MessagePack as JSON",
            Convert = b =>
            {
                var json = MessagePackSerializer.ConvertToJson(b);
                return JsonSerializerService.Instance?.Format(json) ?? string.Empty;
            },
            LanguageExtension = ".jsonc"
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
            },
            LanguageExtension = ".xml"
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

        _textEditor = (TextEditor)this.GetTemplateChild("TextEditor");
        _textMateInstallation = _textEditor.InstallTextMate(_textEditorRegistryOptions);
        SyncTextEditor();

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
        if (!string.IsNullOrEmpty(_content))
        {
            Application.Current?.Clipboard?.SetTextAsync(_content);
        }
    }

    void ReadBuffer()
    {
        var format = SelectedFormat;
        if (format == null)
        {
            throw new InvalidOperationException();
        }

        if ((Buffer?.Length ?? 0) == 0)
        {
            _content = string.Empty;
        }
        else
        {
            try
            {
                _content = format.Convert?.Invoke(Buffer!) ?? string.Empty;
            }
            catch (Exception exception)
            {
                _content = $"<{exception.Message}>";
            }
        }

        SyncTextEditor();
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

    void SyncTextEditor()
    {
        if (_textEditor == null)
        {
            return;
        }

        _textEditor.Text = _content;

        if (SelectedFormat == null)
        {
            return;
        }

        if (_textMateInstallation == null)
        {
            return;
        }

        // Avoid updating the language all the time even without a change!
        if (string.Equals(_currentTextEditorLanguage, SelectedFormat.LanguageExtension))
        {
            return;
        }

        _currentTextEditorLanguage = SelectedFormat.LanguageExtension;

        if (SelectedFormat.LanguageExtension == null)
        {
            _textMateInstallation.SetGrammar(_currentTextEditorLanguage);
        }
        else
        {
            _textMateInstallation.SetGrammar(
                _textEditorRegistryOptions.GetScopeByLanguageId(_textEditorRegistryOptions.GetLanguageByExtension(SelectedFormat.LanguageExtension).Id));
        }
    }
}