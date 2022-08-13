using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using MQTTnet;
using MQTTnetApp.Common;
using ReactiveUI;

namespace MQTTnetApp.Pages.TopicExplorer;

public sealed class TopicExplorerItemViewModel : BaseViewModel
{
    string? _currentPayload;
    TopicExplorerItemMessageViewModel? _selectedMessage;
    bool _trackLatestMessage;

    public TopicExplorerItemViewModel()
    {
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

    public ObservableCollection<TopicExplorerItemMessageViewModel> Messages { get; } = new();

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

        Messages.Add(new TopicExplorerItemMessageViewModel(message));

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
        CurrentPayload = Messages.LastOrDefault()?.PayloadString ?? string.Empty;
    }
}