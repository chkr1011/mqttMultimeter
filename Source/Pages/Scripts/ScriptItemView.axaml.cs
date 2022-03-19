using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit;
using AvaloniaEdit.TextMate;
using TextMateSharp.Grammars;

namespace MQTTnetApp.Pages.Scripts;

public sealed class ScriptItemView : UserControl
{
    readonly TextEditor _textEditor;

    public ScriptItemView()
    {
        InitializeComponent();

        _textEditor = this.FindControl<TextEditor>("PART_Editor");

        var registryOptions = new RegistryOptions(ThemeName.DarkPlus);
        var textMateInstallation = _textEditor.InstallTextMate(registryOptions);
        textMateInstallation.SetGrammar(registryOptions.GetScopeByLanguageId(registryOptions.GetLanguageByExtension(".py").Id));
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        var viewModel = (ScriptItemViewModel)DataContext!;

        viewModel.ReceiveScript += (_, args) =>
        {
            args.Script = _textEditor.Text;
        };
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}