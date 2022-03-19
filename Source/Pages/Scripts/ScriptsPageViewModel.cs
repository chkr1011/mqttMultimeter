using System;
using MQTTnetApp.Common;
using MQTTnetApp.Services.Scripting;

namespace MQTTnetApp.Pages.Scripts;

public sealed class ScriptsPageViewModel : BasePageViewModel
{
    public ScriptsPageViewModel(ScriptEngineService scriptEngineService)
    {
        ScriptEngineService = scriptEngineService ?? throw new ArgumentNullException(nameof(scriptEngineService));
    }

    public PageItemsViewModel<ScriptItemViewModel> Items { get; } = new();

    public ScriptEngineService ScriptEngineService { get; }

    public void AddItem()
    {
        var newItem = new ScriptItemViewModel(this)
        {
            Name = "Untitled"
        };

        Items.Collection.Add(newItem);
        Items.SelectedItem = newItem;
    }
}