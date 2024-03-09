using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using MessagePack;
using mqttMultimeter.Extensions;
using mqttMultimeter.Services.Data;
using mqttMultimeter.Text;
using TextMateSharp.Grammars;

namespace mqttMultimeter.Controls;

public sealed class BufferInspectorView : TemplatedControl
{
    static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true
    };

    public static readonly StyledProperty<byte[]?> BufferProperty = AvaloniaProperty.Register<BufferInspectorView, byte[]?>(nameof(Buffer));

    public static readonly StyledProperty<BufferConverter?> SelectedFormatProperty = AvaloniaProperty.Register<BufferInspectorView, BufferConverter?>(nameof(SelectedFormat));

    public static readonly StyledProperty<IList<BufferConverter>> FormatsProperty = AvaloniaProperty.Register<BufferInspectorView, IList<BufferConverter>>(nameof(Formats));

    public static readonly StyledProperty<bool> ShowRawProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(ShowRaw));

    public static readonly StyledProperty<bool> ShowTextProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(ShowText));

    public static readonly StyledProperty<bool> ShowPictureProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(ShowPicture));

    public static readonly StyledProperty<bool> UseBase64PreDecodingProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(UseBase64PreDecoding));

    public static readonly StyledProperty<string?> SelectedFormatNameProperty = AvaloniaProperty.Register<BufferInspectorView, string?>(nameof(SelectedFormatName), "UTF-8");

    readonly RegistryOptions _textEditorRegistryOptions = new(ThemeName.Dark);
    Button? _copyToClipboardButton;
    HexBox? _hexBox;
    Viewbox? _pictureBox;
    Button? _saveToFileButton;
    TextEditor? _textEditor;
    TextMate.Installation? _textMateInstallation;

    public BufferInspectorView()
    {
        Formats = SharedConverters;

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

    static ObservableCollection<BufferConverter> SharedConverters { get; } =
    [
        new BufferConverter("ASCII", null, b => Encoding.ASCII.GetString(b)),
        new BufferConverter("Base64", null, Convert.ToBase64String),
        new BufferConverter("Binary", null, BinaryEncoder.GetString),
        new BufferConverter("HEX", null, HexEncoder.GetString),
        new BufferConverter("JSON",
            "source.json.comments",
            b =>
            {
                var json = Encoding.UTF8.GetString(b);
                return JsonSerializer.Serialize(JsonNode.Parse(json), JsonSerializerOptions);
            }),


        new BufferConverter("MessagePack as JSON",
            "source.json.comments",
            b =>
            {
                var json = MessagePackSerializer.ConvertToJson(b);
                return JsonSerializerService.Instance?.Format(json) ?? string.Empty;
            }),

        new BufferConverter("Picture", null, _ => "PICTURE"), // Special case!
        new BufferConverter("RAW", null, _ => "RAW"), // Special case!

        new BufferConverter("Sparkplug B",
            "source.json.comments",
            b =>
            {
                var payload = Org.Eclipse.Tahu.Protobuf.Payload.Parser.ParseFrom(b);
                return JsonSerializer.Serialize(payload, JsonSerializerOptions);
            }),

        new BufferConverter("Unicode", null, b => Encoding.Unicode.GetString(b)),
        new BufferConverter("UTF-8", null, b => Encoding.UTF8.GetString(b)),
        new BufferConverter("UTF-32", null, b => Encoding.UTF32.GetString(b)),
        new BufferConverter("XML",
            "text.xml",
            b =>
            {
                var xml = Encoding.UTF8.GetString(b);
                return XDocument.Parse(xml).ToString(SaveOptions.None);
            })
    ];

    public bool ShowPicture
    {
        get => GetValue(ShowPictureProperty);
        set => SetValue(ShowPictureProperty, value);
    }

    public bool ShowRaw
    {
        get => GetValue(ShowRawProperty);
        set => SetValue(ShowRawProperty, value);
    }

    public bool ShowText
    {
        get => GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, value);
    }

    public bool UseBase64PreDecoding
    {
        get => GetValue(UseBase64PreDecodingProperty);
        set => SetValue(UseBase64PreDecodingProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _hexBox = (HexBox)this.GetTemplateChild("HexBox");
        _pictureBox = (Viewbox)this.GetTemplateChild("PictureBox");

        _textEditor = (TextEditor)this.GetTemplateChild("TextEditor");
        _textMateInstallation = _textEditor.InstallTextMate(_textEditorRegistryOptions);

        _copyToClipboardButton = (Button)this.GetTemplateChild("CopyToClipboardButton");
        _copyToClipboardButton.Click += OnCopyToClipboard;

        _saveToFileButton = (Button)this.GetTemplateChild("SaveToFileButton");
        _saveToFileButton.Click += OnSaveToFile;

        Sync();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SelectedFormatProperty)
        {
            if (string.Equals(SelectedFormat?.Name, "RAW"))
            {
                ShowRaw = true;
                ShowText = false;
                ShowPicture = false;
            }
            else if (string.Equals(SelectedFormat?.Name, "Picture"))
            {
                ShowPicture = true;
                ShowText = false;
                ShowRaw = false;
            }
            else
            {
                ShowText = true;
                ShowPicture = false;
                ShowRaw = false;
            }
        }

        if (change.Property == UseBase64PreDecodingProperty || change.Property == SelectedFormatProperty || change.Property == BufferProperty)
        {
            Sync();
        }

        if (change.Property == SelectedFormatNameProperty)
        {
            SelectFormat();
        }
    }

    void OnCopyToClipboard(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(_textEditor!.Text))
        {
            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            _ = clipboard?.SetTextAsync(_textEditor.Text);
        }
    }

    void OnSaveToFile(object? sender, RoutedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            try
            {
                var filePickerOptions = new FilePickerSaveOptions
                {
                    FileTypeChoices = new List<FilePickerFileType>
                    {
                        new("Binary files")
                        {
                            Patterns = new[]
                            {
                                "*.bin"
                            }
                        }
                    }
                };

                var file = await TopLevel.GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(filePickerOptions);
                if (file == null)
                {
                    return;
                }

                await using var stream = await file.OpenWriteAsync();
                await stream.WriteAsync(Buffer ?? Array.Empty<byte>());
            }
            catch (FileNotFoundException)
            {
                // Ignore this case!
            }
            catch (Exception exception)
            {
                App.ShowException(exception);
            }
        });
    }

    void SelectFormat()
    {
        if (string.IsNullOrEmpty(SelectedFormatName))
        {
            SelectedFormat = Formats.FirstOrDefault(i => i.Name.Equals("UTF-8"));
        }
        else
        {
            SelectedFormat = Formats.FirstOrDefault(f => string.Equals(f.Name, SelectedFormatName));
        }

        if (SelectedFormat == null)
        {
            SelectedFormat = Formats.FirstOrDefault();
        }
    }

    void Sync()
    {
        if (_textEditor == null || _hexBox == null || _pictureBox == null)
        {
            return;
        }

        if (SelectedFormat == null)
        {
            return;
        }

        var buffer = Buffer ?? Array.Empty<byte>();
        if (UseBase64PreDecoding)
        {
            try
            {
                buffer = Convert.FromBase64String(Encoding.ASCII.GetString(buffer));
            }
            catch
            {
                // Go ahead if decoding did not work. It is probably not required.
            }
        }

        if (SelectedFormat.Name == "RAW")
        {
            // Only fill the data of the hex box when it is actually used!
            _hexBox.Value = buffer;
        }
        else if (SelectedFormat.Name == "Picture")
        {
            try
            {
                if (_pictureBox.Child is Image existingImage)
                {
                    ((Bitmap)existingImage.Source!).Dispose();
                }

                _pictureBox.Child = new Image
                {
                    Source = new Bitmap(new MemoryStream(buffer))
                };
            }
            catch
            {
                // Ignore. We may can set a picture here indicating that the format is not supported.
                _pictureBox.Child = null;
            }
        }
        else
        {
            string text;
            try
            {
                text = SelectedFormat.Convert(buffer);
            }
            catch (Exception exception)
            {
                text = exception.ToString();
            }

            _textMateInstallation?.SetGrammar(SelectedFormat.Grammar);

            // It is important to set the content after the grammar so that
            // the highlighting gets applied properly!
            _textEditor.Text = text;
        }
    }
}