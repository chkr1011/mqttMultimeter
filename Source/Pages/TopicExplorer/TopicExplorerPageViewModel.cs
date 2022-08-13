using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData;
using DynamicData.Binding;
using MQTTnet;
using MQTTnet.Client;
using MQTTnetApp.Common;
using MQTTnetApp.Services.Mqtt;
using ReactiveUI;

namespace MQTTnetApp.Pages.TopicExplorer;

public sealed class TopicExplorerPageViewModel : BaseViewModel
{
    readonly SourceList<TopicExplorerTreeNodeViewModel> _rootNodes = new();
    bool _isRecordingEnabled;

    TopicExplorerTreeNodeViewModel? _selectedNode;

    public TopicExplorerPageViewModel(MqttClientService mqttClientService)
    {
        mqttClientService.ApplicationMessageReceived += OnMqttMessageReceived;

        _rootNodes.Connect()
            .Sort(SortExpressionComparer<TopicExplorerTreeNodeViewModel>.Ascending(t => t.Name))
            .Bind(out var bindingData)
            .Subscribe();

        Nodes = bindingData;
    }

    public bool IsRecordingEnabled
    {
        get => _isRecordingEnabled;
        set => this.RaiseAndSetIfChanged(ref _isRecordingEnabled, value);
    }

    public ReadOnlyObservableCollection<TopicExplorerTreeNodeViewModel> Nodes { get; }

    public TopicExplorerTreeNodeViewModel? SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    public void Clear()
    {
        _rootNodes.Clear();
    }

    void InsertNode(string[] path, MqttApplicationMessage message)
    {
        var target = _rootNodes;

        var fullTopic = new StringBuilder();
        for (var index = 0; index < path.Length; index++)
        {
            var currentPath = path[index];
            fullTopic.Append(currentPath);
            
            var targetNode = target.Items.FirstOrDefault(i => i.Name.Equals(currentPath));

            if (targetNode == null)
            {
                targetNode = new TopicExplorerTreeNodeViewModel(currentPath);

                target.Add(targetNode);
            }

            target = targetNode.Children;

            if (index == path.Length - 1)
            {
                targetNode.Item.AddMessage(message);
            }

            fullTopic.Append("/");
        }
    }

    Task OnMqttMessageReceived(MqttApplicationMessageReceivedEventArgs arguments)
    {
        if (!_isRecordingEnabled)
        {
            return Task.CompletedTask;
        }

        var topic = arguments.ApplicationMessage.Topic;
        var path = topic.Split("/");

        return Dispatcher.UIThread.InvokeAsync(() => InsertNode(path, arguments.ApplicationMessage));
    }
}