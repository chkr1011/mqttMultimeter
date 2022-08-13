using System;
using System.Collections.ObjectModel;
using DynamicData;
using DynamicData.Binding;
using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.TopicExplorer;

public sealed class TopicExplorerTreeNodeViewModel : BaseViewModel
{
    bool _isExpanded;

    public TopicExplorerTreeNodeViewModel(string name)
    {
        Name = name;

        Children.Connect().Sort(SortExpressionComparer<TopicExplorerTreeNodeViewModel>.Ascending(t => t.Name)).Bind(out var nodes).Subscribe();

        Nodes = nodes;

        Item = new TopicExplorerItemViewModel();
    }

    public SourceList<TopicExplorerTreeNodeViewModel> Children { get; } = new();

    public bool IsExpanded
    {
        get => _isExpanded;
        set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
    }

    public TopicExplorerItemViewModel Item { get; }

    public string Name { get; }

    public ReadOnlyObservableCollection<TopicExplorerTreeNodeViewModel> Nodes { get; }
}