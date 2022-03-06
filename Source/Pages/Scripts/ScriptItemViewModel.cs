using System;
using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.Scripts;

public sealed class ScriptItemViewModel : BaseViewModel
{
    string _name = string.Empty;

    public ScriptItemViewModel(ScriptsPageViewModel ownerPage)
    {
        OwnerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ScriptsPageViewModel OwnerPage { get; }
}