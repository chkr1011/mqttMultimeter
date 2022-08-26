using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData;
using MQTTnet;
using MQTTnet.Client;
using MQTTnetApp.Common;
using MQTTnetApp.Controls;
using MQTTnetApp.Pages.Publish;
using MQTTnetApp.Services.Mqtt;
using ReactiveUI;

namespace MQTTnetApp.Pages.Inflight;

public sealed class InflightPageViewModel : BasePageViewModel
{
    readonly ReadOnlyObservableCollection<InflightPageItemViewModel> _items;
    readonly SourceList<InflightPageItemViewModel> _itemsSource = new();
    readonly MqttClientService _mqttClientService;
    readonly PublishPageViewModel _publishPage;

    string? _filterText;
    bool _isRecordingEnabled = true;
    int _number;

    InflightPageItemViewModel? _selectedItem;

    public InflightPageViewModel(MqttClientService mqttClientService, PublishPageViewModel publishPage)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));
        _publishPage = publishPage;

        mqttClientService.ApplicationMessageReceived += OnApplicationMessageReceived;

        var filter = this.WhenAnyValue(x => x.FilterText).Throttle(TimeSpan.FromMilliseconds(800)).Select(BuildFilter);

        _itemsSource.Connect().Filter(filter).ObserveOn(RxApp.MainThreadScheduler).Bind(out _items).Subscribe();
    }

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

    Func<InflightPageItemViewModel, bool> BuildFilter(string? searchText)
    {
        if (string.IsNullOrEmpty(searchText))
        {
            return t => true;
        }

        return t => t.Topic.Contains(searchText, StringComparison.OrdinalIgnoreCase);
    }

    InflightPageItemViewModel CreateItemViewModel(MqttApplicationMessage applicationMessage)
    {
        var itemViewModel = InflightPageItemViewModel.Create(applicationMessage, _number++);

        itemViewModel.RepeatMessageRequested += (_, __) =>
        {
            RepeatApplicationMessage(itemViewModel);
        };

        itemViewModel.DeleteRetainedMessageRequested += OnDeleteRetainedMessageRequested;

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

        var newItem = CreateItemViewModel(eventArgs.ApplicationMessage);

        return Dispatcher.UIThread.InvokeAsync(() =>
        {
            _itemsSource.Add(newItem);

            // TODO: Move to configuration.
            if (_itemsSource.Count > 1000)
            {
                _itemsSource.RemoveAt(0);
            }
        });
    }

    void OnDeleteRetainedMessageRequested(object? sender, EventArgs e)
    {
        var item = (InflightPageItemViewModel)sender!;
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

    void RepeatApplicationMessage(InflightPageItemViewModel item)
    {
        _publishPage.RepeatMessage(item);
    }
}