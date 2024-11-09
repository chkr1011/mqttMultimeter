using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using mqttMultimeter.Common;
using mqttMultimeter.Pages.Inflight;
using MQTTnet;
using ReactiveUI;

namespace mqttMultimeter.Pages.TopicExplorer;

public sealed class TopicExplorerItemViewModel : BaseViewModel
{
    readonly TopicExplorerPageViewModel _ownerPage;

    long _currentPayloadLength;
    string? _currentPayloadPreview;
    bool _hasPayload;
    DateTime? _lastUpdateTimestamp;
    TopicExplorerItemMessageViewModel? _selectedMessage;
    long _totalPayloadLength;
    bool _trackLatestMessage;

    public TopicExplorerItemViewModel(TopicExplorerPageViewModel ownerPage)
    {
        _ownerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));

        Messages.CollectionChanged += OnMessagesChanged;
    }

    public long CurrentPayloadLength
    {
        get => _currentPayloadLength;
        private set => this.RaiseAndSetIfChanged(ref _currentPayloadLength, value);
    }

    public string? CurrentPayloadPreview
    {
        get => _currentPayloadPreview;
        private set
        {
            this.RaiseAndSetIfChanged(ref _currentPayloadPreview, value);
            this.RaisePropertyChanged(nameof(HasPayload));
        }
    }

    public bool HasPayload
    {
        get => _hasPayload;
        private set => this.RaiseAndSetIfChanged(ref _hasPayload, value);
    }

    public DateTime? LastUpdateTimestamp
    {
        get => _lastUpdateTimestamp;
        private set => this.RaiseAndSetIfChanged(ref _lastUpdateTimestamp, value);
    }

    public CollectionViewModel<TopicExplorerItemMessageViewModel> Messages { get; } = new();

    public TopicExplorerItemMessageViewModel? SelectedMessage
    {
        get => _selectedMessage;
        set => this.RaiseAndSetIfChanged(ref _selectedMessage, value);
    }

    public long TotalPayloadLength
    {
        get => _totalPayloadLength;
        private set => this.RaiseAndSetIfChanged(ref _totalPayloadLength, value);
    }

    public bool TrackLatestMessage
    {
        get => _trackLatestMessage;
        set
        {
            this.RaiseAndSetIfChanged(ref _trackLatestMessage, value);

            // Already jump to the latest message which is currently available.
            SelectedMessage = Messages.LastOrDefault();
        }
    }

    public void AddMessage(MqttApplicationMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }


        var timestamp = DateTime.Now;

        TotalPayloadLength += message.Payload.Length;
        LastUpdateTimestamp = timestamp;

        var duration = TimeSpan.Zero;
        var lastMessage = Messages.LastOrDefault();
        if (lastMessage != null)
        {
            duration = timestamp - lastMessage.Timestamp;
        }

        var viewModel = new TopicExplorerItemMessageViewModel(timestamp, message, duration);
        viewModel.InflightItem.RepeatMessageRequested += OnInflightItemOnRepeatMessageRequested;
        viewModel.InflightItem.DeleteRetainedMessageRequested += OnInflightItemOnDeleteRetainedMessageRequested;

        Messages.Add(viewModel);

        if (TrackLatestMessage)
        {
            SelectedMessage = Messages.Last();
            return;
        }

        SelectedMessage ??= Messages.First();
    }

    public void Clear()
    {
        Messages.Clear();
        SelectedMessage = null;
    }

    void OnInflightItemOnDeleteRetainedMessageRequested(object? s, EventArgs _)
    {
        _ownerPage.DeleteRetainedMessage((InflightPageItemViewModel)s!);
    }

    void OnInflightItemOnRepeatMessageRequested(object? s, EventArgs _)
    {
        _ownerPage.RepeatMessage((InflightPageItemViewModel)s!);
    }

    void OnMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CurrentPayloadPreview = Messages.LastOrDefault()?.PayloadPreview ?? string.Empty;
        CurrentPayloadLength = Messages.LastOrDefault()?.PayloadLength ?? 0;
        HasPayload = CurrentPayloadLength > 0;
    }
}