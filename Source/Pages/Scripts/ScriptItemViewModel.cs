using System;
using System.Threading.Tasks;
using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.Scripts;

public sealed class ScriptItemViewModel : BaseViewModel
{
    string _name = string.Empty;
    string _output = string.Empty;

    public ScriptItemViewModel(ScriptsPageViewModel ownerPage)
    {
        OwnerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));
    }

    public event EventHandler<ReceiveScriptEventArgs>? ReceiveScript;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string Output
    {
        get => _output;
        set => this.RaiseAndSetIfChanged(ref _output, value);
    }

    public ScriptsPageViewModel OwnerPage { get; }

    public void ClearOutput()
    {
        Output = string.Empty;
    }

    public async Task Execute()
    {
        try
        {
            Output = string.Empty;

            var script = GetScript();

            var result = await OwnerPage.ScriptEngineService.Execute(script, t => Output += t);
            Output += Convert.ToString(result);
        }
        catch (Exception exception)
        {
            Output += Convert.ToString(exception);
        }
    }

    public string GetScript()
    {
        var receiveScriptsEventArgs = new ReceiveScriptEventArgs();
        ReceiveScript?.Invoke(this, receiveScriptsEventArgs);

        return receiveScriptsEventArgs.Script ?? string.Empty;
    }

    public void OpenInExternalEditor()
    {
    }

    public void SaveToTextFile()
    {
    }
}