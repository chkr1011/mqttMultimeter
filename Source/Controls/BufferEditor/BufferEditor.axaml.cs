using System;
using System.Xml.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using AvaloniaEdit.TextMate.Grammars;
using MQTTnetApp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MQTTnetApp.Controls;

public sealed class BufferEditor : TemplatedControl
{
    public static readonly StyledProperty<string?> PayloadProperty = AvaloniaProperty.Register<BufferInspectorView, string?>(nameof(Payload));

    public static readonly StyledProperty<bool> IsTextProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsText));

    public static readonly StyledProperty<bool> IsBase64Property = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsBase64));

    public static readonly StyledProperty<bool> IsJsonProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsJson));

    public static readonly StyledProperty<bool> IsXmlProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsXml));

    public static readonly StyledProperty<bool> IsPathProperty = AvaloniaProperty.Register<BufferInspectorView, bool>(nameof(IsPath));

    readonly RegistryOptions _textEditorRegistryOptions = new(ThemeName.Dark);
    Button? _copyToClipboardButton;
    bool _isUpdatingTextInternally;
    Button? _reformatButton;
    Button? _saveToFileButton;

    TextEditor? _textEditor;
    TextMate.Installation? _textMateInstallation;

    public BufferEditor()
    {
        IsText = true;
    }

    public bool IsBase64
    {
        get => GetValue(IsBase64Property);
        set => SetValue(IsBase64Property, value);
    }

    public bool IsPath
    {
        get => GetValue(IsPathProperty);
        set => SetValue(IsPathProperty, value);
    }

    public bool IsJson
    {
        get => GetValue(IsJsonProperty);
        set => SetValue(IsJsonProperty, value);
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

    public string? Payload
    {
        get => GetValue(PayloadProperty);
        set => SetValue(PayloadProperty, value);
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

        _reformatButton = (Button)this.GetTemplateChild("ReformatButton");
        _reformatButton.Click += OnReformat;

        SyncText();
        SyncLanguage();
    }

    protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PayloadProperty)
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
            SyncLanguage();
        }
    }

    void OnCopyToClipboard(object? sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(Payload))
        {
            Application.Current?.Clipboard?.SetTextAsync(Payload);
        }
    }

    void OnReformat(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Payload))
        {
            return;
        }
        
        try
        {
            if (IsJson)
            {
                var json = JToken.Parse(Payload);
                Payload = json.ToString(Formatting.Indented);
            }
            else if (IsXml)
            {
                var xml = XDocument.Parse(Payload);
                Payload = xml.ToString(SaveOptions.None);
            }
        }
        catch
        {
            // Ignore all errors when trying to reformat.
        }
    }

    void OnSaveToFile(object? sender, RoutedEventArgs e)
    {
        // TODO:
    }

    void OnTextEditorTextChanged(object? sender, EventArgs e)
    {
        var text = _textEditor!.Text;

        _isUpdatingTextInternally = true;
        try
        {
            Payload = text;
        }
        finally
        {
            _isUpdatingTextInternally = false;
        }
    }

    void SyncLanguage()
    {
        if (_textMateInstallation == null)
        {
            return;
        }

        if (IsJson)
        {
            _textMateInstallation.SetGrammar(_textEditorRegistryOptions.GetScopeByLanguageId("jsonc"));
        }
        else if (IsXml)
        {
            _textMateInstallation.SetGrammar(_textEditorRegistryOptions.GetScopeByLanguageId("xml"));
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

        _textEditor.Text = Payload;
    }
}