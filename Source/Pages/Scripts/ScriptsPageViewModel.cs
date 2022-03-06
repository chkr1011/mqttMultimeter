using MQTTnetApp.Common;

namespace MQTTnetApp.Pages.Scripts;

public sealed class ScriptsPageViewModel : BasePageViewModel
{
    public PageItemsViewModel<ScriptsPageViewModel> Items { get; } = new();
}