using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData;
using DynamicData.Binding;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using mqttMultimeter.Pages.Inflight;
using mqttMultimeter.Services.Mqtt;
using MQTTnet;
using MQTTnet.Client;
using ReactiveUI;

namespace mqttMultimeter.Pages.TopicExplorer;

public sealed class TopicExplorerPageViewModel : BasePageViewModel
{
    readonly MqttClientService _mqttClientService;
    readonly SourceList<TopicExplorerTreeNodeViewModel> _rootNodes = new();

    bool _highlightChanges = true;
    bool _isRecordingEnabled;

    TopicExplorerTreeNodeViewModel? _selectedNode;

    public TopicExplorerPageViewModel(MqttClientService mqttClientService)
    {
        if (mqttClientService == null)
        {
            throw new ArgumentNullException(nameof(mqttClientService));
        }

        if (mqttClientService == null)
        {
            throw new ArgumentNullException(nameof(mqttClientService));
        }

        mqttClientService.ApplicationMessageReceived += OnMqttMessageReceived;

        _mqttClientService = mqttClientService;

        _rootNodes.Connect()
            .Sort(SortExpressionComparer<TopicExplorerTreeNodeViewModel>.Ascending(t => t.Name))
            .Bind(out var bindingData)
            .Subscribe(_ => this.RaisePropertyChanged(nameof(HasNodes)));

        Nodes = bindingData;
    }

    public event Action<InflightPageItemViewModel>? RepeatMessageRequested;

    public bool HasNodes => Nodes.Count > 0;

    public bool HighlightChanges
    {
        get => _highlightChanges;
        set => this.RaiseAndSetIfChanged(ref _highlightChanges, value);
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
        SelectedNode = null;
    }

    public void CollapseAll()
    {
        foreach (var node in Nodes)
        {
            SetExpandedState(node, false);
        }
    }

    public void DeleteRetainedMessage(InflightPageItemViewModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        OverlayContent = ProgressIndicatorViewModel.Create($"Deleting retained message...\r\n\r\n{item.Topic}");

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            try
            {
                var message = new MqttApplicationMessageBuilder().WithTopic(item.Topic)
                    .WithQualityOfServiceLevel(item.QualityOfServiceLevel)
                    .WithPayload(ArraySegment<byte>.Empty)
                    .Build();

                await _mqttClientService.Publish(message, CancellationToken.None);
            }
            catch (Exception exception)
            {
                App.ShowException(exception);
            }
            finally
            {
                OverlayContent = null;
            }
        });
    }

    public void ExpandSelectedTree()
    {
        if (_selectedNode == null)
        {
            return;
        }

        SetExpandedState(_selectedNode, true);
    }

    public void RepeatMessage(InflightPageItemViewModel item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        RepeatMessageRequested?.Invoke(item);
    }

    void InsertNode(string[] path, MqttApplicationMessage message)
    {
        var target = _rootNodes;
        TopicExplorerTreeNodeViewModel? parentNode = null;

        var fullTopic = new StringBuilder();
        for (var index = 0; index < path.Length; index++)
        {
            var currentPath = path[index];
            fullTopic.Append(currentPath);

            var targetNode = target.Items.FirstOrDefault(i => i.Name.Equals(currentPath));
            if (targetNode == null)
            {
                targetNode = new TopicExplorerTreeNodeViewModel(currentPath, parentNode, this);
                target.Add(targetNode);
            }

            target = targetNode.NodesSource;

            if (index == path.Length - 1)
            {
                targetNode.AddMessage(message);
            }

            fullTopic.Append('/');

            parentNode = targetNode;
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

    static void SetExpandedState(TopicExplorerTreeNodeViewModel node, bool value)
    {
        node.IsExpanded = value;

        foreach (var childNode in node.Nodes)
        {
            SetExpandedState(childNode, value);
        }
    }
}