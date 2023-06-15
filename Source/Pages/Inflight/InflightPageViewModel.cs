using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData;
using mqttMultimeter.Common;
using mqttMultimeter.Controls;
using mqttMultimeter.Main;
using mqttMultimeter.Pages.Inflight.Export;
using mqttMultimeter.Services.Mqtt;
using MQTTnet;
using MQTTnet.Client;
using ReactiveUI;

namespace mqttMultimeter.Pages.Inflight;

public sealed class InflightPageViewModel : BasePageViewModel
{
    const string ExportDialogTitle = "Export Inflight items";

    static readonly FileDialogFilter ExportDialogFilter = new()
    {
        Name = "MQTT Inflight item exports",
        Extensions = new List<string>
        {
            "mqtt_inflight"
        }
    };

    readonly InflightPageItemExportService _exportService;
    readonly ReadOnlyObservableCollection<InflightPageItemViewModel> _items;
    readonly SourceList<InflightPageItemViewModel> _itemsSource = new();
    readonly MqttClientService _mqttClientService;

    long _counter;

    string? _filterText;
    bool _isRecordingEnabled = true;

    InflightPageItemViewModel? _selectedItem;

    public InflightPageViewModel(MqttClientService mqttClientService, InflightPageItemExportService exportService)
    {
        _mqttClientService = mqttClientService ?? throw new ArgumentNullException(nameof(mqttClientService));
        _exportService = exportService ?? throw new ArgumentNullException(nameof(exportService));

        mqttClientService.ApplicationMessageReceived += OnApplicationMessageReceived;

        var filter = this.WhenAnyValue(x => x.FilterText).Throttle(TimeSpan.FromMilliseconds(800)).Select(BuildFilter);

        _itemsSource.Connect().Filter(filter).ObserveOn(RxApp.MainThreadScheduler).Bind(out _items).Subscribe();
    }

    public event Action<InflightPageItemViewModel>? RepeatMessageRequested;

    public long Counter => _counter;

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

    public Task AppendMessage(MqttApplicationMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var newItem = CreateItemViewModel(message);

        return Dispatcher.UIThread.InvokeAsync(() =>
        {
            _itemsSource.Add(newItem);

            // TODO: Move to configuration.
            if (_itemsSource.Count > 1000)
            {
                _itemsSource.RemoveAt(0);
            }
        }).GetTask();
    }

    public void ClearItems()
    {
        _itemsSource.Clear();
    }

    public async Task ExportItems()
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = ExportDialogTitle
        };

        saveFileDialog.Filters?.Add(ExportDialogFilter);

        var fileName = await saveFileDialog.ShowAsync(MainWindow.Instance);
        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }

        await _exportService.Export(this, fileName);
    }

    public async Task ImportItems()
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = ExportDialogTitle
        };

        openFileDialog.Filters?.Add(ExportDialogFilter);

        var fileName = (await openFileDialog.ShowAsync(MainWindow.Instance))?.FirstOrDefault();
        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }

        if (!File.Exists(fileName))
        {
            return;
        }

        await _exportService.Import(this, fileName);
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
        this.RaisePropertyChanged(nameof(Counter));

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
        RepeatMessageRequested?.Invoke(item);
    }
}