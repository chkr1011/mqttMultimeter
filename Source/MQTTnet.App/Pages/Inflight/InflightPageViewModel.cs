using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData;
using MQTTnet.App.Common;
using MQTTnet.App.Pages.Publish;
using MQTTnet.App.Services.Mqtt;
using MQTTnet.Client;
using ReactiveUI;

namespace MQTTnet.App.Pages.Inflight;

public sealed class InflightPageViewModel : BaseViewModel
{
    readonly ReadOnlyObservableCollection<InflightPageItemViewModel> _items;
    readonly SourceList<InflightPageItemViewModel> _itemsSource = new();
    readonly PublishPageViewModel _publishPage;

    string? _filterText;
    bool _isRecordingEnabled = true;
    int _number;

    InflightPageItemViewModel? _selectedItem;

    public InflightPageViewModel(MqttClientService mqttClientService, PublishPageViewModel publishPage)
    {
        if (mqttClientService == null)
        {
            throw new ArgumentNullException(nameof(mqttClientService));
        }

        _publishPage = publishPage;

        mqttClientService.ApplicationMessageReceived += OnApplicationMessageReceived;

        var filterTextChanged = this.WhenAnyValue(x => x.FilterText).Throttle(TimeSpan.FromMilliseconds(800)).ObserveOn(RxApp.MainThreadScheduler);
        var filter = new Func<InflightPageItemViewModel, bool>(i => string.IsNullOrEmpty(_filterText) || i.Topic.Contains(_filterText));

        _itemsSource.Connect().RefCount().AutoRefreshOnObservable(_ => filterTextChanged).Filter(filter).Bind(out _items).Subscribe();
    }

    public event EventHandler? SwitchToPublishRequested;

    public string? FilterText
    {
        get => _filterText;
        set => this.RaiseAndSetIfChanged(ref _filterText, value);
    }

    public bool IsRecordingEnabled
    {
        get => _isRecordingEnabled;
        set => this.RaiseAndSetIfChanged(ref _isRecordingEnabled, value);
    }

    public ReadOnlyObservableCollection<InflightPageItemViewModel> Items => _items;

    public InflightPageItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public void ClearItems()
    {
        _itemsSource.Clear();
    }

    public void RepeatItem(InflightPageItemViewModel item)
    {
        var publishItem = new PublishItemViewModel(_publishPage)
        {
            Name = $"Repeat '{item.Topic}'",
            ContentType = item.Source.ContentType,
            Topic = item.Topic,
            Payload = item.PayloadPreview
        };

        _publishPage.Items.Add(publishItem);
        _publishPage.SelectedItem = publishItem;

        SwitchToPublishRequested?.Invoke(this, EventArgs.Empty);
    }

    InflightPageItemViewModel CreateViewModel(MqttApplicationMessage applicationMessage)
    {
        var itemViewModel = new InflightPageItemViewModel
        {
            OwnerPage = this,
            Timestamp = DateTime.Now,
            Number = _number++,
            Topic = applicationMessage.Topic,
            Length = applicationMessage.Payload?.Length ?? 0L,
            Retained = applicationMessage.Retain,
            Source = applicationMessage,
            QualityOfServiceLevel = applicationMessage.QualityOfServiceLevel,
            UserProperties =
            {
                IsReadOnly = true
            }
        };

        itemViewModel.UserProperties.Load(applicationMessage.UserProperties);

        return itemViewModel;
    }

    Task OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        if (!_isRecordingEnabled)
        {
            // The recording is currently disabled. All received message are getting
            // lost and will not be shown when enabled again.
            return Task.CompletedTask;
        }

        var newItem = CreateViewModel(eventArgs.ApplicationMessage);

        return Dispatcher.UIThread.InvokeAsync(() =>
        {
            _itemsSource.Insert(0, newItem);
        });
    }
}