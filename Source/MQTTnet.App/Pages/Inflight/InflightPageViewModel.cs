using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using DynamicData;
using MQTTnet.App.Common;
using MQTTnet.App.Services.Mqtt;
using MQTTnet.Client;
using ReactiveUI;

namespace MQTTnet.App.Pages.Inflight;

public sealed class InflightPageViewModel : BaseViewModel
{
    readonly ReadOnlyObservableCollection<InflightPageItemViewModel> _items;
    readonly SourceList<InflightPageItemViewModel> _itemsSource = new();

    string _filterText;
    int _number;

    InflightPageItemViewModel? _selectedItem;

    public InflightPageViewModel(MqttClientService mqttClientService)
    {
        if (mqttClientService == null)
        {
            throw new ArgumentNullException(nameof(mqttClientService));
        }

        mqttClientService.ApplicationMessageReceived += OnApplicationMessageReceived;

        var filterTextChanged = this.WhenAnyValue(x => x.FilterText).Throttle(TimeSpan.FromMilliseconds(800)).ObserveOn(RxApp.MainThreadScheduler);
        var filter = new Func<InflightPageItemViewModel, bool>(i => string.IsNullOrEmpty(_filterText) || i.Topic.Contains(_filterText));

        _itemsSource.Connect().RefCount().AutoRefreshOnObservable(_ => filterTextChanged).Filter(filter).Bind(out _items).Subscribe();
    }

    public string FilterText
    {
        get => _filterText;
        set => this.RaiseAndSetIfChanged(ref _filterText, value);
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

    InflightPageItemViewModel CreateViewModel(MqttApplicationMessage applicationMessage)
    {
        var itemViewModel = new InflightPageItemViewModel
        {
            Timestamp = DateTime.Now,
            Number = _number++,
            Topic = applicationMessage.Topic,
            Length = applicationMessage.Payload?.Length ?? 0L,
            Retained = applicationMessage.Retain,
            Source = applicationMessage,
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
        return Dispatcher.UIThread.InvokeAsync(() =>
        {
            _itemsSource.Add(CreateViewModel(eventArgs.ApplicationMessage));
        });
    }
}