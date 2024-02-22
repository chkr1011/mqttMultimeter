using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using mqttMultimeter.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TextMateSharp.Grammars;

namespace mqttMultimeter.Controls;

public sealed class BufferEditor : TemplatedControl
{
    static readonly List<FilePickerFileType> FileDialogFilters = new()
    {
        new FilePickerFileType("JSON files")
        {
            Patterns = new List<string>
            {
                "*.json"
            }
        },
        new FilePickerFileType("Text files")
        {
            Patterns = new List<string>
            {
                "*.txt"
            }
        },
        new FilePickerFileType("XML files")
        {
            Patterns = new List<string>
            {
                "*.xml"
            }
        },
        new FilePickerFileType("All files")
        {
            Patterns = new List<string>
            {
                "*"
            }
        }
    };

    public static readonly StyledProperty<string?> BufferProperty = AvaloniaProperty.Register<BufferInspectorView, string?>(nameof(Buffer));

    public static readonly StyledProperty<bool> IsTextProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsText), true);

    public static readonly StyledProperty<bool> IsBase64Property = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsBase64));

    public static readonly StyledProperty<bool> IsJsonProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsJson));

    public static readonly StyledProperty<bool> IsXmlProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsXml));

    public static readonly StyledProperty<bool> IsPathProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsPath));

    public static readonly StyledProperty<BufferFormat> BufferFormatProperty = AvaloniaProperty.Register<BufferInspectorView, BufferFormat>(nameof(BufferFormat));

    readonly RegistryOptions _textEditorRegistryOptions = new(ThemeName.Dark);

    Button? _copyToClipboardButton;
    bool _isUpdatingTextInternally;
    Button? _loadFromFileButton;
    Button? _reformatButton;
    Button? _saveToFileButton;

    TextEditor? _textEditor;
    TextMate.Installation? _textMateInstallation;

    public BufferEditor()
    {
        IsText = true;
    }

    public string? Buffer
    {
        get => GetValue(BufferProperty);
        set => SetValue(BufferProperty, value);
    }

    public BufferFormat BufferFormat
    {
        get => GetValue(BufferFormatProperty);
        set => SetValue(BufferFormatProperty, value);
    }

    public bool IsBase64
    {
        get => GetValue(IsBase64Property);
        set => SetValue(IsBase64Property, value);
    }

    public bool IsJson
    {
        get => GetValue(IsJsonProperty);
        set => SetValue(IsJsonProperty, value);
    }

    public bool IsPath
    {
        get => GetValue(IsPathProperty);
        set => SetValue(IsPathProperty, value);
    }

    public bool IsText
    {
        get => GetValue(IsTextProperty);
        set => SetValue(IsTextProperty, value);
    }

    public bool IsXml
    {
        get => GetValue(IsXmlProperty);
        set => SetValue(IsXmlProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _textEditor = (TextEditor)this.GetTemplateChild("TextEditor");
        _textEditor.TextChanged += OnTextEditorTextChanged;
        _textMateInstallation = _textEditor.InstallTextMate(_textEditorRegistryOptions);

        _copyToClipboardButton = (Button)this.GetTemplateChild("CopyToClipboardButton");
        _copyToClipboardButton.Click += OnCopyToClipboard;

        _saveToFileButton = (Button)this.GetTemplateChild("SaveToFileButton");
        _saveToFileButton.Click += OnSaveToFile;

        _loadFromFileButton = (Button)this.GetTemplateChild("LoadFromFileButton");
        _loadFromFileButton.Click += OnLoadFromFile;

        _reformatButton = (Button)this.GetTemplateChild("ReformatButton");
        _reformatButton.Click += OnReformat;

        var increaseFontSizeButton = (Button)this.GetTemplateChild("IncreaseFontSizeButton");
        increaseFontSizeButton.Click += (_, __) =>
        {
            _textEditor.FontSize += 0.5;
        };

        var decreaseFontSizeButton = (Button)this.GetTemplateChild("DecreaseFontSizeButton");
        decreaseFontSizeButton.Click += (_, __) =>
        {
            _textEditor.FontSize -= 0.5;
        };

        SyncText();
        SyncGrammar();
        SyncBufferFormat();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == BufferProperty)
        {
            if (!_isUpdatingTextInternally)
            {
                SyncText();
            }
        }
        else if (change.Property == IsTextProperty ||
                 change.Property == IsBase64Property ||
                 change.Property == IsJsonProperty ||
                 change.Property == IsXmlProperty ||
                 change.Property == IsPathProperty)
        {
            if (change.NewValue is true)
            {
                SyncGrammar();
                SyncBufferFormat();
            }
        }
    }

    void OnCopyToClipboard(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Buffer))
        {
            var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
            _ = clipboard?.SetTextAsync(Buffer);
        }
    }

    void OnLoadFromFile(object? sender, RoutedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var filePickerOptions = new FilePickerOpenOptions
            {
                FileTypeFilter = FileDialogFilters
            };

            var files = await TopLevel.GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(filePickerOptions);
            if (files.Count == 0)
            {
                return;
            }

            try
            {
                using (var stream = new StreamReader(await files[0].OpenReadAsync()))
                {
                    Buffer = await stream.ReadToEndAsync();
                }
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

    void OnReformat(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Buffer))
        {
            return;
        }

        try
        {
            if (IsJson)
            {
                var json = JToken.Parse(Buffer);
                Buffer = json.ToString(Formatting.Indented);
            }
            else if (IsXml)
            {
                var xml = XDocument.Parse(Buffer);
                Buffer = xml.ToString(SaveOptions.None);
            }
        }
        catch
        {
            // Ignore all errors when trying to reformat.
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
                    FileTypeChoices = FileDialogFilters
                };

                // TODO: Not working!
                if (IsJson)
                {
                    filePickerOptions.DefaultExtension = ".json";
                }
                else if (IsXml)
                {
                    filePickerOptions.DefaultExtension = ".xml";
                }
                else
                {
                    filePickerOptions.DefaultExtension = ".txt";
                }

                var file = await TopLevel.GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(filePickerOptions);
                if (file == null)
                {
                    return;
                }

                await using (var stream = new StreamWriter(await file.OpenWriteAsync()))
                {
                    await stream.WriteAsync(Buffer ?? string.Empty);
                }
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

    void OnTextEditorTextChanged(object? sender, EventArgs e)
    {
        var text = _textEditor!.Text;

        _isUpdatingTextInternally = true;
        try
        {
            Buffer = text;
        }
        finally
        {
            _isUpdatingTextInternally = false;
        }
    }

    void SyncBufferFormat()
    {
        if (IsText || IsJson || IsXml)
        {
            BufferFormat = BufferFormat.Plain;
        }
        else if (IsBase64)
        {
            BufferFormat = BufferFormat.Base64;
        }
        else if (IsPath)
        {
            BufferFormat = BufferFormat.Path;
        }

        // It may happen that noting is selected while the selection is changed!
    }

    void SyncGrammar()
    {
        if (_textMateInstallation == null)
        {
            return;
        }

        if (IsJson)
        {
            _textMateInstallation.SetGrammar("source.json.comments");
        }
        else if (IsXml)
        {
            _textMateInstallation.SetGrammar("text.xml");
        }
        else
        {
            _textMateInstallation.SetGrammar(null);
        }
    }

    void SyncText()
    {
        if (_textEditor == null)
        {
            return;
        }

        _textEditor.Text = Buffer;
    }
}