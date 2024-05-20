using System.Collections.ObjectModel;
using ReactiveUI;

namespace mqttMultimeter.Common;

public sealed class PageItemsViewModel<TItem> : BaseViewModel
{
    TItem? _selectedItem;

    public ObservableCollection<TItem> Collection { get; } = new();

    public TItem? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public void Clear()
    {
        Collection.Clear();
        SelectedItem = default;
    }

    public void MoveItemDown(object? item)
    {
        if (item == null)
        {
            return;
        }

        var index = Collection.IndexOf((TItem)item);
        Move(index, ++index);
    }

    public void MoveItemUp(object? item)
    {
        if (item == null)
        {
            return;
        }

        var index = Collection.IndexOf((TItem)item);
        Move(index, --index);
    }

    public void RemoveItem(object? item)
    {
        if (item == null)
        {
            return;
        }

        var index = Collection.IndexOf((TItem)item);
        Collection.Remove((TItem)item);

        if (index > 0)
        {
            SelectedItem = Collection[--index];
        }
    }

    void Move(int from, int to)
    {
        if (to == -1)
        {
            return;
        }

        if (from == -1)
        {
            return;
        }

        if (to == Collection.Count)
        {
            return;
        }

        var restoreSelection = ReferenceEquals(Collection[from], SelectedItem);
        Collection.Move(from, to);

        if (restoreSelection)
        {
            SelectedItem = Collection[to];
        }
    }
}