using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using MQTTnet;
using MQTTnetApp.Common;
using MQTTnetApp.Pages.Inflight;
using ReactiveUI;

namespace MQTTnetApp.Pages.TopicExplorer;

public sealed class TopicExplorerItemViewModel : BaseViewModel
{
    readonly TopicExplorerPageViewModel _ownerPage;

    string? _currentPayload;
    TopicExplorerItemMessageViewModel? _selectedMessage;
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

    public bool HasPayload => CurrentPayload != null;

    public CollectionViewModel<TopicExplorerItemMessageViewModel> Messages { get; } = new();

    public TopicExplorerItemMessageViewModel? SelectedMessage
    {
        get => _selectedMessage;
        set => this.RaiseAndSetIfChanged(ref _selectedMessage, value);
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
            payload = Encoding.UTF8.GetString(message.Payload ?? ReadOnlySpan<byte>.Empty);
        }
        catch
        {
            // Ignore error.
            payload = string.Empty;
        }

        var timestamp = DateTime.Now;
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
    }
}