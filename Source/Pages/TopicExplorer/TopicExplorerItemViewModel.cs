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

    string? _currentPayload;
    int _currentPayloadLength;
    DateTime? _lastUpdateTimestamp;
    TopicExplorerItemMessageViewModel? _selectedMessage;
    int _totalPayloadLength;
    bool _trackLatestMessage;

    public TopicExplorerItemViewModel(TopicExplorerPageViewModel ownerPage)
    {
        _ownerPage = ownerPage ?? throw new ArgumentNullException(nameof(ownerPage));

        Messages.CollectionChanged += OnMessagesChanged;
    }

    public string? CurrentPayload
    {
        get => _currentPayload;
        private set
        {
            this.RaiseAndSetIfChanged(ref _currentPayload, value);
            this.RaisePropertyChanged(nameof(HasPayload));
        }
    }

    public int CurrentPayloadLength
    {
        get => _currentPayloadLength;
        private set => this.RaiseAndSetIfChanged(ref _currentPayloadLength, value);
    }

    public bool HasPayload => CurrentPayload != null;

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

    public int TotalPayloadLength
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

        string payload;
        try
        {
            payload = Encoding.UTF8.GetString(message.PayloadSegment);
        }
        catch
        {
            // Ignore error.
            payload = string.Empty;
        }

        var timestamp = DateTime.Now;

        TotalPayloadLength += message.PayloadSegment.Count;
        LastUpdateTimestamp = timestamp;

        var duration = TimeSpan.Zero;
        var lastMessage = Messages.LastOrDefault();
        if (lastMessage != null)
        {
            duration = timestamp - lastMessage.Timestamp;
        }

        var viewModel = new TopicExplorerItemMessageViewModel(timestamp, message, payload, duration);
        viewModel.InflightItem.RepeatMessageRequested += (s, _) => _ownerPage.RepeatMessage((InflightPageItemViewModel)s!);
        viewModel.InflightItem.DeleteRetainedMessageRequested += (s, _) => _ownerPage.DeleteRetainedMessage((InflightPageItemViewModel)s!);

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

    void OnMessagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        CurrentPayload = Messages.LastOrDefault()?.Payload ?? string.Empty;
        CurrentPayloadLength = Messages.LastOrDefault()?.PayloadLength ?? 0;
    }
}