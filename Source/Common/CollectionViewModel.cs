using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace mqttMultimeter.Common;

public sealed class CollectionViewModel<TItem> : ObservableCollection<TItem>
{
    public CollectionViewModel()
    {
        CollectionChanged += OnCollectionChanged;
    }

    public bool IsEmpty => Count == 0;

    void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsEmpty)));
    }
}