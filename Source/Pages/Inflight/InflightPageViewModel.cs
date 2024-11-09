using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData;
using DynamicData.Binding;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using mqttMultimeter.Pages.Inflight.Export;
using mqttMultimeter.Services.Mqtt;
using MQTTnet;
using ReactiveUI;

namespace mqttMultimeter.Pages.Inflight;

public sealed class InflightPageViewModel : BasePageViewModel
{
    readonly InflightPageItemExportService _exportService;
    readonly ReadOnlyObservableCollection<InflightPageItemViewModel> _items;
    readonly SourceList<InflightPageItemViewModel> _itemsSource = new();
    readonly MqttClientService _mqttClientService;

    long _counter;

    string? _filterText;
    bool _isRecordingEnabled = true;

    public InflightPageViewModel(MqttClientService mqttClientService, InflightPageItemExportService exportService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));
        _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));

        mqttClientService.ApplicationMessageReceived += OnApplicationMessageReceived;

        var filter = this.WhenAnyValue(x => x.FilterText).Throttle(TimeSpan.FromMilliseconds(800)).Select(BuildFilter);

        _itemsSource.Connect()
            .Filter(filter)
            .Sort(SortExpressionComparer<InflightPageItemViewModel>.Ascending(t => t.Number))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _items)
            .Subscribe();
    }

    public event Action<InflightPageItemViewModel>? RepeatMessageRequested;

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

    public Task AppendMessage(MqttApplicationMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        return Dispatcher.UIThread.InvokeAsync(() =>
            {
                var newItem = CreateItemViewModel(message);

                _itemsSource.Add(newItem);

                // TODO: Move to configuration.
                if (_itemsSource.Count > 1000)
                {
                    _itemsSource.RemoveAt(0);
                }
            })
            .GetTask();
    }

    public void ClearItems()
    {
        _itemsSource.Clear();
    }

    public Task ExportItems(string path)
    {
        return _exportService.Export(this, path);
    }

    public Task ImportItems(string path)
    {
        if (!File.Exists(path))
        {
            return Task.CompletedTask;
        }

        return _exportService.Import(this, path);
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
        var counter = Interlocked.Increment(ref _counter);

        var itemViewModel = InflightPageItemViewModelFactory.Create(applicationMessage, counter);

        itemViewModel.RepeatMessageRequested += (_, _) =>
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

        return AppendMessage(eventArgs.ApplicationMessage);
    }

    void OnDeleteRetainedMessageRequested(object? sender, EventArgs e)
    {
        var item = (InflightPageItemViewModel)sender!;
        OverlayContent = ProgressIndicatorViewModel.Create($"Deleting retained message...\r\n\r\n{item.Topic}");

        Dispatcher.UIThread.InvokeAsync(async () =>
        {
            try
            {
                // To delete a retained message it is important to set the body to an empty
                // one and setting the retain flag to _true_ as well!
                var message = new MqttApplicationMessageBuilder().WithTopic(item.Topic)
                    .WithQualityOfServiceLevel(item.QualityOfServiceLevel)
                    .WithPayload(ArraySegment<byte>.Empty)
                    .WithRetainFlag()
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
        RepeatMessageRequested?.Invoke(item);
    }
}