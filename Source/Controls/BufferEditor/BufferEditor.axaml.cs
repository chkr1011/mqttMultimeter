using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using AvaloniaEdit.TextMate.Grammars;
using mqttMultimeter.Extensions;
using mqttMultimeter.Main;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace mqttMultimeter.Controls;

public sealed class BufferEditor : TemplatedControl
{
    static readonly List<FileDialogFilter> FileDialogFilters = new()
    {
        new FileDialogFilter
        {
            Name = "Text files",
            Extensions = new List<string>
            {
                "txt"
            }
        },
        new FileDialogFilter
        {
            Name = "JSON files",
            Extensions = new List<string>
            {
                "json"
            }
        },
        new FileDialogFilter
        {
            Name = "XML files",
            Extensions = new List<string>
            {
                "xml"
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
            SyncGrammar();
            SyncBufferFormat();
        }
    }

    void OnCopyToClipboard(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Buffer))
        {
            Application.Current?.Clipboard?.SetTextAsync(Buffer);
        }
    }

    void OnLoadFromFile(object? sender, RoutedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filters!.AddRange(FileDialogFilters);

            var fileName = (await openFileDialog.ShowAsync(MainWindow.Instance))?.FirstOrDefault();
            if (!string.IsNullOrEmpty(fileName))
            {
                try
                {
                    Buffer = await File.ReadAllTextAsync(fileName, CancellationToken.None);
                }
                catch (FileNotFoundException)
                {
                    // Ignore this case!
                }
                catch (Exception exception)
                {
                    App.ShowException(exception);
                }
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
        var saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filters!.AddRange(FileDialogFilters);

        // TODO: Not working!
        if (IsJson)
        {
            saveFileDialog.DefaultExtension = ".json";
        }
        else if (IsXml)
        {
            saveFileDialog.DefaultExtension = ".xml";
        }
        else
        {
            saveFileDialog.DefaultExtension = ".txt";
        }

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var fileName = await saveFileDialog.ShowAsync(MainWindow.Instance);
            if (!string.IsNullOrEmpty(fileName))
            {
                await File.WriteAllTextAsync(fileName, Buffer ?? string.Empty, CancellationToken.None);
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