using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DynamicData;
using DynamicData.Binding;
using mqttMultimeter.Common;
using MQTTnet;
using ReactiveUI;

namespace mqttMultimeter.Pages.TopicExplorer;

public sealed class TopicExplorerTreeNodeViewModel : BaseViewModel
{
    bool _isExpanded;

    public TopicExplorerTreeNodeViewModel(string name, TopicExplorerTreeNodeViewModel? parent, TopicExplorerPageViewModel ownerPage)
    {
        OwnerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Parent = parent;

        NodesSource.Connect().Sort(SortExpressionComparer<TopicExplorerTreeNodeViewModel>.Ascending(t => t.Name)).Bind(out var nodes).Subscribe();

        Nodes = nodes;

        Item = new TopicExplorerItemViewModel(OwnerPage);
        Item.Messages.CollectionChanged += OnMessagesChanged;
    }

    public event EventHandler? MessagesChanged;

    public bool IsExpanded
    {
        get => _isExpanded;
        set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
    }

    public TopicExplorerItemViewModel Item { get; }

    public string Name { get; }

    public ReadOnlyObservableCollection<TopicExplorerTreeNodeViewModel> Nodes { get; }

    public SourceList<TopicExplorerTreeNodeViewModel> NodesSource { get; } = new();

    public TopicExplorerPageViewModel OwnerPage { get; }

    TopicExplorerTreeNodeViewModel? Parent { get; }

    public void AddMessage(MqttApplicationMessage message)
    {
        Item.AddMessage(message);
    }

    public void Clear()
    {
        Item.Clear();
    }

    void OnMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Parent?.OnMessagesChanged(sender, e);
        MessagesChanged?.Invoke(sender, e);
    }
}